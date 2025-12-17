using API_de_Reservas.Models;

namespace API_de_Reservas.DALs
{
    public class AutenticacionRepository : IAutenticacionRepository
    {
        private readonly ApplicationDbContext _context;

        public AutenticacionRepository(ApplicationDbContext context) { 
            _context = context;
        }

        public async Task<Usuario> Registro(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }
    }
}
