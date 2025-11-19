using Microsoft.EntityFrameworkCore;
using NovaData;
using NovaModel;

namespace NovaBusiness
{
    public class SkillService
    {
        private readonly ApplicationDbContext _context;
        private readonly RecomendacaoService _recomendacaoService;

        public SkillService(ApplicationDbContext context, RecomendacaoService recomendacaoService)
        {
            _context = context;
            _recomendacaoService = recomendacaoService;
        }

        public async Task<List<SkillModel>> ListarPorUsuario(int usuarioId)
        {
            return await _context.Skills
                .Where(s => s.UsuarioId == usuarioId)
                .OrderBy(s => s.Nome)
                .ToListAsync();
        }

        public async Task<SkillModel?> ObterPorId(int id)
        {
            return await _context.Skills
                .Include(s => s.Usuario)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<SkillModel> Criar(SkillModel skill)
        {
            _context.Skills.Add(skill);
            await _context.SaveChangesAsync();

            // Gerar recomendações automaticamente
            await _recomendacaoService.GerarRecomendacoesPorSkill(skill);

            return skill;
        }

        public async Task<SkillModel?> Atualizar(int id, SkillModel skill)
        {
            var skillExistente = await _context.Skills.FindAsync(id);
            if (skillExistente == null)
                return null;

            skillExistente.Nome = skill.Nome;
            skillExistente.Nivel = skill.Nivel;

            await _context.SaveChangesAsync();

            // Gerar novas recomendações se o nível mudou
            await _recomendacaoService.GerarRecomendacoesPorSkill(skillExistente);

            return skillExistente;
        }

        public async Task<bool> Deletar(int id)
        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill == null)
                return false;

            _context.Skills.Remove(skill);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}