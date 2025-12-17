using API_de_Reservas.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace API_de_Reservas.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PasswordHash {  get; set; }
        public UsuarioRol Rol { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public ICollection<Reserva> Reservas { get; set; }
    }
}
