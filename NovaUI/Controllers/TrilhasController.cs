using Microsoft.AspNetCore.Mvc;
using NovaBusiness;
using NovaBusiness.DTOs;
using NovaModel;

namespace NovaUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TrilhasController : ControllerBase
    {
        private readonly TrilhaService _trilhaService;

        public TrilhasController(TrilhaService trilhaService)
        {
            _trilhaService = trilhaService;
        }

        /// <summary>
        /// Lista todas as trilhas (regras de recomendação)
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TrilhaResponseDTO>>> GetAll()
        {
            var trilhas = await _trilhaService.ListarTodas();
            var trilhasDTO = trilhas.Select(t => MapToResponseDTO(t));

            return Ok(new
            {
                data = trilhasDTO,
                links = new
                {
                    self = Url.Action(nameof(GetAll)),
                    create = Url.Action(nameof(Create))
                }
            });
        }

        /// <summary>
        /// Busca trilha por ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TrilhaResponseDTO>> GetById(int id)
        {
            var trilha = await _trilhaService.ObterPorId(id);

            if (trilha == null)
            {
                return NotFound(new ProblemDetails
                {
                    Status = 404,
                    Title = "Trilha não encontrada",
                    Detail = $"Não existe trilha com ID {id}",
                    Instance = HttpContext.Request.Path
                });
            }

            var trilhaDTO = MapToResponseDTO(trilha);

            return Ok(new
            {
                data = trilhaDTO,
                links = new
                {
                    self = Url.Action(nameof(GetById), new { id }),
                    update = Url.Action(nameof(Update), new { id }),
                    delete = Url.Action(nameof(Delete), new { id }),
                    all = Url.Action(nameof(GetAll))
                }
            });
        }

        /// <summary>
        /// Cria uma nova trilha (regra de recomendação)
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TrilhaResponseDTO>> Create([FromBody] TrilhaCreateUpdateDTO trilhaDTO)
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
                var trilha = MapToModel(trilhaDTO);
                var trilhaCriada = await _trilhaService.Criar(trilha);
                var responseDTO = MapToResponseDTO(trilhaCriada);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = trilhaCriada.Id },
                    new
                    {
                        data = responseDTO,
                        links = new
                        {
                            self = Url.Action(nameof(GetById), new { id = trilhaCriada.Id }),
                            update = Url.Action(nameof(Update), new { id = trilhaCriada.Id }),
                            delete = Url.Action(nameof(Delete), new { id = trilhaCriada.Id })
                        }
                    }
                );
            }
            catch (Exception ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = 400,
                    Title = "Erro ao criar trilha",
                    Detail = ex.Message,
                    Instance = HttpContext.Request.Path
                });
            }
        }

        /// <summary>
        /// Atualiza uma trilha existente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TrilhaResponseDTO>> Update(int id, [FromBody] TrilhaCreateUpdateDTO trilhaDTO)
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
                var trilha = MapToModel(trilhaDTO);
                var trilhaAtualizada = await _trilhaService.Atualizar(id, trilha);

                if (trilhaAtualizada == null)
                {
                    return NotFound(new ProblemDetails
                    {
                        Status = 404,
                        Title = "Trilha não encontrada",
                        Detail = $"Não foi possível atualizar. Trilha com ID {id} não existe.",
                        Instance = HttpContext.Request.Path
                    });
                }

                var responseDTO = MapToResponseDTO(trilhaAtualizada);

                return Ok(new
                {
                    data = responseDTO,
                    links = new
                    {
                        self = Url.Action(nameof(GetById), new { id }),
                        delete = Url.Action(nameof(Delete), new { id })
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = 400,
                    Title = "Erro ao atualizar trilha",
                    Detail = ex.Message,
                    Instance = HttpContext.Request.Path
                });
            }
        }

        /// <summary>
        /// Remove uma trilha
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var sucesso = await _trilhaService.Deletar(id);

            if (!sucesso)
            {
                return NotFound(new ProblemDetails
                {
                    Status = 404,
                    Title = "Trilha não encontrada",
                    Detail = $"Não foi possível deletar. Trilha com ID {id} não existe.",
                    Instance = HttpContext.Request.Path
                });
            }

            return NoContent();
        }

        private TrilhaResponseDTO MapToResponseDTO(TrilhaModel trilha)
        {
            return new TrilhaResponseDTO
            {
                Id = trilha.Id,
                AreaInteresse = trilha.AreaInteresse,
                SkillRelacionada = trilha.SkillRelacionada,
                NivelMinimo = trilha.NivelMinimo,
                TituloRecomendacao = trilha.TituloRecomendacao,
                DescricaoRecomendacao = trilha.DescricaoRecomendacao
            };
        }

        private TrilhaModel MapToModel(TrilhaCreateUpdateDTO dto)
        {
            return new TrilhaModel
            {
                AreaInteresse = dto.AreaInteresse,
                SkillRelacionada = dto.SkillRelacionada,
                NivelMinimo = dto.NivelMinimo,
                TituloRecomendacao = dto.TituloRecomendacao,
                DescricaoRecomendacao = dto.DescricaoRecomendacao
            };
        }
    }
}