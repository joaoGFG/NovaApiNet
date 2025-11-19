using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaBusiness.DTOs
{
    public class TrilhaCreateUpdateDTO
    {
        [Required]
        [MaxLength(80)]
        public string AreaInteresse { get; set; } = string.Empty;

        [Required]
        [MaxLength(80)]
        public string SkillRelacionada { get; set; } = string.Empty;

        [Required]
        [Range(1, 5)]
        public int NivelMinimo { get; set; }

        [Required]
        [MaxLength(120)]
        public string TituloRecomendacao { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string DescricaoRecomendacao { get; set; } = string.Empty;
    }

    public class TrilhaResponseDTO
    {
        public int Id { get; set; }
        public string AreaInteresse { get; set; } = string.Empty;
        public string SkillRelacionada { get; set; } = string.Empty;
        public int NivelMinimo { get; set; }
        public string TituloRecomendacao { get; set; } = string.Empty;
        public string DescricaoRecomendacao { get; set; } = string.Empty;
    }
}
