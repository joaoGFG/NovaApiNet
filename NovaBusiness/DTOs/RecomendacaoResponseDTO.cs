using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaBusiness.DTOs
{
    public class RecomendacaoResponseDTO
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public DateTime CriadaEm { get; set; }
    }
}
