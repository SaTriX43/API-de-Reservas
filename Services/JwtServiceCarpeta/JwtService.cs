using API_de_Reservas.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_de_Reservas.Services.JwtServiceCarpeta
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerarToken(Usuario usuario)
        {
            var jwt = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]));

            // 2. Claims del usuario
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Role, usuario.Rol.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
            };

            // 3. Crear credenciales de firma
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 4. Descriptor del token (la plantilla)
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30),
                Issuer = jwt["Issuer"],
                Audience = jwt["Audience"],
                SigningCredentials = creds
            };

            // 5. Crear el token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // 6. Convertir a string
            return tokenHandler.WriteToken(token);
        }
    }
}
