using API_de_Reservas.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace API_de_Reservas.Models
{
    public class Recurso
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public TipoRecurso Tipo { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public ICollection<Reserva> Reservas { get; set; }
    }
}
