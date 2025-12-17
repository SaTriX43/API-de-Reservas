using API_de_Reservas.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace API_de_Reservas.DTOs.ReservaDtoCarpeta
{
    public class ReservaCrearDto
    {
        [Required]
        public int UsuarioId { get; set; }
        [Required]
        public int RecursoId { get; set; }
        [Required]
        public DateTime FechaInicio { get; set; }
        [Required]
        public DateTime FechaFinal { get; set; }
    }
}
