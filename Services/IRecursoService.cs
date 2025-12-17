using API_de_Reservas.DTOs.RecursoDtoCarpeta;
using API_de_Reservas.Models;

namespace API_de_Reservas.Services
{
    public interface IRecursoService
    {
        public Task<Result<RecursoDto>> CrearRecurso(RecursoCrearDto recursoCrearDto);
    }
}
