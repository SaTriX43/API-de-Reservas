using API_de_Reservas.Models;
using API_de_Reservas.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace API_de_Reservas.DTOs.UsuarioDtoCarpeta
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public UsuarioRol Rol { get; set; }
        public DateTime FechaCreacion { get; set; } 
    }
}
