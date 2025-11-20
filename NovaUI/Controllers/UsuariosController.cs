using Microsoft.AspNetCore.Mvc;
using NovaBusiness;
using NovaBusiness.DTOs;
using NovaModel;

namespace NovaUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuariosController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        /// <summary>
        /// Lista todos os usuários
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UsuarioResponseDTO>>> GetAll()
        {
            var usuarios = await _usuarioService.ListarTodos();
            var usuariosDTO = usuarios.Select(u => MapToResponseDTO(u));
            return Ok(usuariosDTO);
        }

        /// <summary>
        /// Busca usuário por ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UsuarioResponseDTO>> GetById(int id)
        {
            var usuario = await _usuarioService.ObterPorId(id);

            if (usuario == null)
            {
                return NotFound(new ProblemDetails
                {
                    Status = 404,
                    Title = "Usuário não encontrado",
                    Detail = $"Não existe usuário com ID {id}",
                    Instance = HttpContext.Request.Path
                });
            }

            var usuarioDTO = MapToResponseDTO(usuario);

            // HATEOAS - adicionar links
            var response = new
            {
                data = usuarioDTO,
                links = new
                {
                    self = Url.Action(nameof(GetById), new { id }),
                    update = Url.Action(nameof(Update), new { id }),
                    delete = Url.Action(nameof(Delete), new { id }),
                    skills = Url.Action("GetByUsuario", "Skills", new { usuarioId = id }),
                    recomendacoes = Url.Action("GetByUsuario", "Recomendacoes", new { usuarioId = id })
                }
            };

            return Ok(response);
        }

        /// <summary>
        /// Cria um novo usuário
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UsuarioResponseDTO>> Create([FromBody] UsuarioCreateUpdateDTO usuarioDTO)
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
                var usuario = MapToModel(usuarioDTO);
                var usuarioCriado = await _usuarioService.Criar(usuario);
                var responseDTO = MapToResponseDTO(usuarioCriado);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = usuarioCriado.Id },
                    new
                    {
                        data = responseDTO,
                        links = new
                        {
                            self = Url.Action(nameof(GetById), new { id = usuarioCriado.Id }),
                            update = Url.Action(nameof(Update), new { id = usuarioCriado.Id }),
                            delete = Url.Action(nameof(Delete), new { id = usuarioCriado.Id })
                        }
                    }
                );
            }
            catch (Exception ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = 400,
                    Title = "Erro ao criar usuário",
                    Detail = ex.Message,
                    Instance = HttpContext.Request.Path
                });
            }
        }

        /// <summary>
        /// Atualiza um usuário existente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UsuarioResponseDTO>> Update(int id, [FromBody] UsuarioCreateUpdateDTO usuarioDTO)
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
                var usuario = MapToModel(usuarioDTO);
                var usuarioAtualizado = await _usuarioService.Atualizar(id, usuario);

                if (usuarioAtualizado == null)
                {
                    return NotFound(new ProblemDetails
                    {
                        Status = 404,
                        Title = "Usuário não encontrado",
                        Detail = $"Não foi possível atualizar. Usuário com ID {id} não existe.",
                        Instance = HttpContext.Request.Path
                    });
                }

                var responseDTO = MapToResponseDTO(usuarioAtualizado);
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
                    Title = "Erro ao atualizar usuário",
                    Detail = ex.Message,
                    Instance = HttpContext.Request.Path
                });
            }
        }

        /// <summary>
        /// Remove um usuário
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var sucesso = await _usuarioService.Deletar(id);

            if (!sucesso)
            {
                return NotFound(new ProblemDetails
                {
                    Status = 404,
                    Title = "Usuário não encontrado",
                    Detail = $"Não foi possível deletar. Usuário com ID {id} não existe.",
                    Instance = HttpContext.Request.Path
                });
            }

            return NoContent();
        }

        /// <summary>
        /// Busca usuários com filtros, paginação e ordenação (15 pts)
        /// </summary>
        /// <param name="nome">Nome do usuário (contém)</param>
        /// <param name="areaInteresse">Área de interesse</param>
        /// <param name="pageNumber">Número da página (padrão: 1)</param>
        /// <param name="pageSize">Tamanho da página (padrão: 10, máx: 100)</param>
        /// <param name="orderBy">Ordenação: nome, nome_desc, data, data_desc</param>
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Search(
            [FromQuery] string? nome,
            [FromQuery] string? areaInteresse,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? orderBy = null)
        {
            // Validações de paginação
            if (pageNumber < 1)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = 400,
                    Title = "Parâmetro inválido",
                    Detail = "pageNumber deve ser maior que 0",
                    Instance = HttpContext.Request.Path
                });
            }

            if (pageSize < 1 || pageSize > 100)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = 400,
                    Title = "Parâmetro inválido",
                    Detail = "pageSize deve estar entre 1 e 100",
                    Instance = HttpContext.Request.Path
                });
            }

            var usuarios = await _usuarioService.Buscar(nome, areaInteresse, pageNumber, pageSize, orderBy);
            var usuariosDTO = usuarios.Select(u => MapToResponseDTO(u));

            // HATEOAS - Links de navegação
            var response = new
            {
                data = usuariosDTO,
                pagination = new
                {
                    currentPage = pageNumber,
                    pageSize,
                    totalItems = usuariosDTO.Count(),
                    hasNext = usuariosDTO.Count() == pageSize,
                    hasPrevious = pageNumber > 1
                },
                links = new
                {
                    self = Url.Action(nameof(Search), new { nome, areaInteresse, pageNumber, pageSize, orderBy }),
                    first = Url.Action(nameof(Search), new { nome, areaInteresse, pageNumber = 1, pageSize, orderBy }),
                    previous = pageNumber > 1
                        ? Url.Action(nameof(Search), new { nome, areaInteresse, pageNumber = pageNumber - 1, pageSize, orderBy })
                        : null,
                    next = usuariosDTO.Count() == pageSize
                        ? Url.Action(nameof(Search), new { nome, areaInteresse, pageNumber = pageNumber + 1, pageSize, orderBy })
                        : null
                }
            };

            return Ok(response);
        }

        // Métodos auxiliares de mapeamento
        private UsuarioResponseDTO MapToResponseDTO(UsuarioModel usuario)
        {
            return new UsuarioResponseDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                AreaInteresse = usuario.AreaInteresse,
                ObjetivoProfissional = usuario.ObjetivoProfissional,
                CriadoEm = usuario.CriadoEm,
                Skills = usuario.Skills?.Select(s => new SkillResponseDTO
                {
                    Id = s.Id,
                    UsuarioId = s.UsuarioId,
                    Nome = s.Nome,
                    Nivel = s.Nivel
                }).ToList() ?? new List<SkillResponseDTO>(),
                Recomendacoes = usuario.Recomendacoes?.Select(r => new RecomendacaoResponseDTO
                {
                    Id = r.Id,
                    UsuarioId = r.UsuarioId,
                    Titulo = r.Titulo,
                    Descricao = r.Descricao,
                    CriadaEm = r.CriadaEm
                }).ToList() ?? new List<RecomendacaoResponseDTO>()
            };
        }

        private UsuarioModel MapToModel(UsuarioCreateUpdateDTO dto)
        {
            return new UsuarioModel
            {
                Nome = dto.Nome,
                Email = dto.Email,
                AreaInteresse = dto.AreaInteresse,
                ObjetivoProfissional = dto.ObjetivoProfissional
            };
        }
    }
}