using API_de_Reservas.Models;

namespace API_de_Reservas.DALs
{
    public interface IRecursoRepository
    {

        public Task<Recurso?> ObtenerRecursoPorNombre(string nombreRecurso);
        public Task<Recurso?> ObtenerRecursoPorId(int recursoId);
        public Task<Recurso> CrearRecurso(Recurso recurso);
    }
}
