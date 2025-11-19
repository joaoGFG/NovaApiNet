using Microsoft.EntityFrameworkCore;
using NovaData;
using NovaModel;

namespace NovaBusiness
{
    public class RecomendacaoService
    {
        private readonly ApplicationDbContext _context;

        public RecomendacaoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<RecomendacaoModel>> ListarPorUsuario(int usuarioId)
        {
            return await _context.Recomendacoes
                .Where(r => r.UsuarioId == usuarioId)
                .OrderByDescending(r => r.CriadaEm)
                .ToListAsync();
        }

        public async Task GerarRecomendacoesPorSkill(SkillModel skill)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == skill.UsuarioId);

            if (usuario == null)
                return;

            // Buscar trilhas compatíveis
            var trilhasCompativeis = await _context.Trilhas
                .Where(t => (t.AreaInteresse == usuario.AreaInteresse || string.IsNullOrEmpty(usuario.AreaInteresse))
                         && t.SkillRelacionada == skill.Nome
                         && t.NivelMinimo <= skill.Nivel)
                .ToListAsync();

            foreach (var trilha in trilhasCompativeis)
            {
                // Verificar se já existe recomendação igual
                var jaExiste = await _context.Recomendacoes
                    .AnyAsync(r => r.UsuarioId == usuario.Id
                                && r.Titulo == trilha.TituloRecomendacao);

                if (!jaExiste)
                {
                    var recomendacao = new RecomendacaoModel
                    {
                        UsuarioId = usuario.Id,
                        Titulo = trilha.TituloRecomendacao,
                        Descricao = trilha.DescricaoRecomendacao,
                        CriadaEm = DateTime.Now
                    };

                    _context.Recomendacoes.Add(recomendacao);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}