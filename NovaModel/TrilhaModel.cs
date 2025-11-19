using System.ComponentModel.DataAnnotations;

namespace NovaModel
{
    public class TrilhaModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(80)]
        public string AreaInteresse { get; set; }

        [Required]
        [MaxLength(80)]
        public string SkillRelacionada { get; set; }

        [Range(1, 5)]
        public int NivelMinimo { get; set; }

        [Required]
        [MaxLength(120)]
        public string TituloRecomendacao { get; set; }

        [Required]
        [MaxLength(500)]
        public string DescricaoRecomendacao { get; set; }
    }
}
