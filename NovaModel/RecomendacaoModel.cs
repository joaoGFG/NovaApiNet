using System;
using System.ComponentModel.DataAnnotations;

namespace NovaModel
{
    public class RecomendacaoModel
    {
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        [MaxLength(120)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string Descricao { get; set; } = string.Empty;

        public DateTime CriadaEm { get; set; } = DateTime.Now;

        // Relacionamento
        public UsuarioModel Usuario { get; set; } = null!;
    }
}