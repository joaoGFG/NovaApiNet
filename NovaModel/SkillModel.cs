using System.ComponentModel.DataAnnotations;

namespace NovaModel
{
    public class SkillModel
    {
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        [MaxLength(80)]
        public string Nome { get; set; }

        [Range(1, 5)]
        public int Nivel { get; set; }

        // Relacionamento
        public UsuarioModel Usuario { get; set; }
    }
}
