using API_de_Reservas.Models;

namespace API_de_Reservas.DALs.RecursoRepositoryCarpeta
{
    public interface IRecursoRepository
    {

        public Task<Recurso?> ObtenerRecursoPorNombreAsync(string nombreRecurso);
        public Task<Recurso?> ObtenerRecursoPorIdAsync(int recursoId);
        public Recurso CrearRecurso(Recurso recurso);
    }
}
