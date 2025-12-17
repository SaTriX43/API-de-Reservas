using API_de_Reservas.Models;
using API_de_Reservas.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace API_de_Reservas.DTOs.ReservaDtoCarpeta
{
    public class ReservaDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int RecursoId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public EstadoReserva Estado { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    }
}
