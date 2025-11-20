using Microsoft.AspNetCore.Mvc;
using NovaBusiness;
using NovaBusiness.DTOs;

namespace NovaUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class RecomendacoesController : ControllerBase
    {
        private readonly RecomendacaoService _recomendacaoService;

        public RecomendacoesController(RecomendacaoService recomendacaoService)
        {
            _recomendacaoService = recomendacaoService;
        }

        /// <summary>
        /// Lista todas as recomendações de um usuário
        /// </summary>
        [HttpGet("usuario/{usuarioId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RecomendacaoResponseDTO>>> GetByUsuario(int usuarioId)
        {
            var recomendacoes = await _recomendacaoService.ListarPorUsuario(usuarioId);
            var recomendacoesDTO = recomendacoes.Select(r => new RecomendacaoResponseDTO
            {
                Id = r.Id,
                UsuarioId = r.UsuarioId,
                Titulo = r.Titulo,
                Descricao = r.Descricao,
                CriadaEm = r.CriadaEm
            });

            return Ok(new
            {
                data = recomendacoesDTO,
                count = recomendacoesDTO.Count(),
                message = recomendacoesDTO.Any()
                    ? "Recomendações geradas com base nas suas skills!"
                    : "Nenhuma recomendação ainda. Adicione skills para receber sugestões personalizadas.",
                links = new
                {
                    self = Url.Action(nameof(GetByUsuario), new { usuarioId }),
                    usuario = Url.Action("GetById", "Usuarios", new { id = usuarioId }),
                    skills = Url.Action("GetByUsuario", "Skills", new { usuarioId })
                }
            });
        }
    }
}