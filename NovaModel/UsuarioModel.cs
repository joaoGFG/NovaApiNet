using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NovaModel
{
    public class UsuarioModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(120)]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; }

        [MaxLength(80)]
        public string AreaInteresse { get; set; }

        [MaxLength(200)]
        public string ObjetivoProfissional { get; set; }

        public DateTime CriadoEm { get; set; } = DateTime.Now;

        public List<SkillModel> Skills { get; set; } = new();
        public List<RecomendacaoModel> Recomendacoes { get; set; } = new();
    }
}
