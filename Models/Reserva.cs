using API_de_Reservas.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace API_de_Reservas.Models
{
    public class Reserva
{
        [Key]
        public int Id { get; set; }
        [Required]
        public int UsuarioId { get; set; }
        [Required]
        public int RecursoId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public EstadoReserva Estado { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public Usuario Usuario { get; set; }
        public Recurso Recurso { get; set; }
    }
}
