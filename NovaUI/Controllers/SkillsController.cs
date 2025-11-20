using Microsoft.AspNetCore.Mvc;
using NovaBusiness;
using NovaBusiness.DTOs;
using NovaModel;

namespace NovaUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class SkillsController : ControllerBase
    {
        private readonly SkillService _skillService;

        public SkillsController(SkillService skillService)
        {
            _skillService = skillService;
        }

        /// <summary>
        /// Lista skills de um usuário específico
        /// </summary>
        [HttpGet("usuario/{usuarioId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SkillResponseDTO>>> GetByUsuario(int usuarioId)
        {
            var skills = await _skillService.ListarPorUsuario(usuarioId);
            var skillsDTO = skills.Select(s => MapToResponseDTO(s));

            return Ok(new
            {
                data = skillsDTO,
                links = new
                {
                    self = Url.Action(nameof(GetByUsuario), new { usuarioId }),
                    usuario = Url.Action("GetById", "Usuarios", new { id = usuarioId }),
                    create = Url.Action(nameof(Create), new { usuarioId })
                }
            });
        }

        /// <summary>
        /// Busca skill por ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SkillResponseDTO>> GetById(int id)
        {
            var skill = await _skillService.ObterPorId(id);

            if (skill == null)
            {
                return NotFound(new ProblemDetails
                {
                    Status = 404,
                    Title = "Skill não encontrada",
                    Detail = $"Não existe skill com ID {id}",
                    Instance = HttpContext.Request.Path
                });
            }

            var skillDTO = MapToResponseDTO(skill);

            return Ok(new
            {
                data = skillDTO,
                links = new
                {
                    self = Url.Action(nameof(GetById), new { id }),
                    update = Url.Action(nameof(Update), new { id }),
                    delete = Url.Action(nameof(Delete), new { id }),
                    usuario = Url.Action("GetById", "Usuarios", new { id = skill.UsuarioId })
                }
            });
        }

        /// <summary>
        /// Cria uma nova skill para um usuário (gera recomendações automaticamente)
        /// </summary>
        [HttpPost("usuario/{usuarioId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SkillResponseDTO>> Create(int usuarioId, [FromBody] SkillCreateUpdateDTO skillDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState)
                {
                    Status = 400,
                    Title = "Erro de validação",
                    Instance = HttpContext.Request.Path
                });
            }

            try
            {
                var skill = new SkillModel
                {
                    UsuarioId = usuarioId,
                    Nome = skillDTO.Nome,
                    Nivel = skillDTO.Nivel
                };

                var skillCriada = await _skillService.Criar(skill);
                var responseDTO = MapToResponseDTO(skillCriada);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = skillCriada.Id },
                    new
                    {
                        data = responseDTO,
                        message = "Skill criada com sucesso! Recomendações foram geradas automaticamente.",
                        links = new
                        {
                            self = Url.Action(nameof(GetById), new { id = skillCriada.Id }),
                            usuario = Url.Action("GetById", "Usuarios", new { id = usuarioId }),
                            recomendacoes = Url.Action("GetByUsuario", "Recomendacoes", new { usuarioId })
                        }
                    }
                );
            }
            catch (Exception ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = 400,
                    Title = "Erro ao criar skill",
                    Detail = ex.Message,
                    Instance = HttpContext.Request.Path
                });
            }
        }

        /// <summary>
        /// Atualiza uma skill existente (regenera recomendações)
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SkillResponseDTO>> Update(int id, [FromBody] SkillCreateUpdateDTO skillDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState)
                {
                    Status = 400,
                    Title = "Erro de validação",
                    Instance = HttpContext.Request.Path
                });
            }

            try
            {
                var skill = new SkillModel
                {
                    Nome = skillDTO.Nome,
                    Nivel = skillDTO.Nivel
                };

                var skillAtualizada = await _skillService.Atualizar(id, skill);

                if (skillAtualizada == null)
                {
                    return NotFound(new ProblemDetails
                    {
                        Status = 404,
                        Title = "Skill não encontrada",
                        Detail = $"Não foi possível atualizar. Skill com ID {id} não existe.",
                        Instance = HttpContext.Request.Path
                    });
                }

                var responseDTO = MapToResponseDTO(skillAtualizada);

                return Ok(new
                {
                    data = responseDTO,
                    message = "Skill atualizada com sucesso! Novas recomendações podem ter sido geradas.",
                    links = new
                    {
                        self = Url.Action(nameof(GetById), new { id }),
                        delete = Url.Action(nameof(Delete), new { id }),
                        recomendacoes = Url.Action("GetByUsuario", "Recomendacoes", new { usuarioId = skillAtualizada.UsuarioId })
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = 400,
                    Title = "Erro ao atualizar skill",
                    Detail = ex.Message,
                    Instance = HttpContext.Request.Path
                });
            }
        }

        /// <summary>
        /// Remove uma skill
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var sucesso = await _skillService.Deletar(id);

            if (!sucesso)
            {
                return NotFound(new ProblemDetails
                {
                    Status = 404,
                    Title = "Skill não encontrada",
                    Detail = $"Não foi possível deletar. Skill com ID {id} não existe.",
                    Instance = HttpContext.Request.Path
                });
            }

            return NoContent();
        }

        private SkillResponseDTO MapToResponseDTO(SkillModel skill)
        {
            return new SkillResponseDTO
            {
                Id = skill.Id,
                UsuarioId = skill.UsuarioId,
                Nome = skill.Nome,
                Nivel = skill.Nivel
            };
        }
    }
}