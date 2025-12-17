using API_de_Reservas.Models;
using API_de_Reservas.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace API_de_Reservas.DTOs.RecursoDtoCarpeta
{
    public class RecursoCrearDto
    {
        [Required]
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        [Required]
        public TipoRecurso Tipo { get; set; }
    }
}
