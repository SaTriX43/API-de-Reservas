using API_de_Reservas.Models;
using Microsoft.EntityFrameworkCore;

namespace API_de_Reservas.DALs.RecursoRepositoryCarpeta
{
    public class RecursoRepository : IRecursoRepository
    {
        private readonly ApplicationDbContext _context;

        public RecursoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Recurso?> ObtenerRecursoPorNombreAsync(string nombreRecurso)
        {
            var recursoEncontrado = await _context.Recursos.FirstOrDefaultAsync(r => r.Nombre ==  nombreRecurso);
            return recursoEncontrado;
        }

        public async Task<Recurso?> ObtenerRecursoPorIdAsync(int recursoId)
        {
            var recursoEncontrado = await _context.Recursos
                .Include(r => r.Reservas)
                .FirstOrDefaultAsync(r => r.Id == recursoId);
            return recursoEncontrado;
        }
        public Recurso CrearRecurso(Recurso recurso)
        {
            _context.Recursos.Add(recurso);
            return recurso;
        }
    }
}
