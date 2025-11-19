using System.ComponentModel.DataAnnotations;

namespace NovaBusiness.DTOs
{
    // DTO para criar/atualizar skill
    public class SkillCreateUpdateDTO
    {
        [Required(ErrorMessage = "Nome da skill é obrigatório")]
        [MaxLength(80)]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nível é obrigatório")]
        [Range(1, 5, ErrorMessage = "Nível deve estar entre 1 e 5")]
        public int Nivel { get; set; }
    }

    // DTO para resposta
    public class SkillResponseDTO
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int Nivel { get; set; }
    }
}
