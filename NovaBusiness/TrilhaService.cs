using Microsoft.EntityFrameworkCore;
using NovaData;
using NovaModel;

namespace NovaBusiness
{
    public class TrilhaService
    {
        private readonly ApplicationDbContext _context;

        public TrilhaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TrilhaModel>> ListarTodas()
        {
            return await _context.Trilhas
                .OrderBy(t => t.AreaInteresse)
                .ThenBy(t => t.SkillRelacionada)
                .ToListAsync();
        }

        public async Task<TrilhaModel?> ObterPorId(int id)
        {
            return await _context.Trilhas.FindAsync(id);
        }

        public async Task<TrilhaModel> Criar(TrilhaModel trilha)
        {
            _context.Trilhas.Add(trilha);
            await _context.SaveChangesAsync();
            return trilha;
        }

        public async Task<TrilhaModel?> Atualizar(int id, TrilhaModel trilha)
        {
            var trilhaExistente = await _context.Trilhas.FindAsync(id);
            if (trilhaExistente == null)
                return null;

            trilhaExistente.AreaInteresse = trilha.AreaInteresse;
            trilhaExistente.SkillRelacionada = trilha.SkillRelacionada;
            trilhaExistente.NivelMinimo = trilha.NivelMinimo;
            trilhaExistente.TituloRecomendacao = trilha.TituloRecomendacao;
            trilhaExistente.DescricaoRecomendacao = trilha.DescricaoRecomendacao;

            await _context.SaveChangesAsync();
            return trilhaExistente;
        }

        public async Task<bool> Deletar(int id)
        {
            var trilha = await _context.Trilhas.FindAsync(id);
            if (trilha == null)
                return false;

            _context.Trilhas.Remove(trilha);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}