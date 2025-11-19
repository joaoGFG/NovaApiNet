using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaBusiness.DTOs
{
    // DTO para criar/atualizar usuário
    public class UsuarioCreateUpdateDTO
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [MaxLength(120)]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(80)]
        public string? AreaInteresse { get; set; }

        [MaxLength(200)]
        public string? ObjetivoProfissional { get; set; }
    }

    // DTO para resposta (inclui dados completos)
    public class UsuarioResponseDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? AreaInteresse { get; set; }
        public string? ObjetivoProfissional { get; set; }
        public DateTime CriadoEm { get; set; }
        public List<SkillResponseDTO> Skills { get; set; } = new();
        public List<RecomendacaoResponseDTO> Recomendacoes { get; set; } = new();
    }
}
