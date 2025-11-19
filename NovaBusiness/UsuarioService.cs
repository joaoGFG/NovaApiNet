using Microsoft.EntityFrameworkCore;
using NovaData;
using NovaModel;

namespace NovaBusiness
{
    public class UsuarioService
    {
        private readonly ApplicationDbContext _context;

        public UsuarioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<UsuarioModel>> ListarTodos()
        {
            return await _context.Usuarios
                .Include(u => u.Skills)
                .Include(u => u.Recomendacoes)
                .ToListAsync();
        }

        public async Task<UsuarioModel?> ObterPorId(int id)
        {
            return await _context.Usuarios
                .Include(u => u.Skills)
                .Include(u => u.Recomendacoes)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<UsuarioModel> Criar(UsuarioModel usuario)
        {
            usuario.CriadoEm = DateTime.Now;
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<UsuarioModel?> Atualizar(int id, UsuarioModel usuario)
        {
            var usuarioExistente = await _context.Usuarios.FindAsync(id);
            if (usuarioExistente == null)
                return null;

            usuarioExistente.Nome = usuario.Nome;
            usuarioExistente.Email = usuario.Email;
            usuarioExistente.AreaInteresse = usuario.AreaInteresse;
            usuarioExistente.ObjetivoProfissional = usuario.ObjetivoProfissional;

            await _context.SaveChangesAsync();
            return usuarioExistente;
        }

        public async Task<bool> Deletar(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return false;

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<UsuarioModel>> Buscar(string? nome, string? areaInteresse, int pageNumber, int pageSize, string? orderBy)
        {
            var query = _context.Usuarios
                .Include(u => u.Skills)
                .Include(u => u.Recomendacoes)
                .AsQueryable();

            // Filtros
            if (!string.IsNullOrWhiteSpace(nome))
                query = query.Where(u => u.Nome.Contains(nome));

            if (!string.IsNullOrWhiteSpace(areaInteresse))
                query = query.Where(u => u.AreaInteresse != null && u.AreaInteresse.Contains(areaInteresse));

            // Ordenação
            query = orderBy?.ToLower() switch
            {
                "nome" => query.OrderBy(u => u.Nome),
                "nome_desc" => query.OrderByDescending(u => u.Nome),
                "data" => query.OrderBy(u => u.CriadoEm),
                "data_desc" => query.OrderByDescending(u => u.CriadoEm),
                _ => query.OrderBy(u => u.Id)
            };

            // Paginação
            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}