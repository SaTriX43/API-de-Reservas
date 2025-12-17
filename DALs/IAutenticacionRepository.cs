using API_de_Reservas.Models;

namespace API_de_Reservas.DALs
{
    public interface IAutenticacionRepository
    {
        public Task<Usuario> Registro(Usuario usuario);
    }
}
