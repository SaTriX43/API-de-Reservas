using API_de_Reservas.Models;
using API_de_Reservas.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace API_de_Reservas.DTOs.RecursoDtoCarpeta
{
    public class RecursoDto
    {
        
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public TipoRecurso Tipo { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    }
}
