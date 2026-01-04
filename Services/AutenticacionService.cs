using API_de_Reservas.DALs.AutenticacionRepositoryCarpeta;
using API_de_Reservas.DALs.UsuarioRepositoryCarpeta;
using API_de_Reservas.DTOs.AutenticacionDtoCarpeta;
using API_de_Reservas.DTOs.UsuarioDtoCarpeta;
using API_de_Reservas.Models;
using API_de_Reservas.Models.Enums;

namespace API_de_Reservas.Services
{
    public class AutenticacionService : IAutenticacionService
    {
        private readonly IAutenticacionRepository _autenticacionRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;

        public AutenticacionService(
            IAutenticacionRepository autenticacionRepository,
            IUsuarioRepository usuarioRepository,
            IJwtService jwtService,
            IConfiguration configuration
            )
        {
            _autenticacionRepository = autenticacionRepository;
            _usuarioRepository = usuarioRepository;
            _jwtService = jwtService;
            _configuration = configuration;
        }

        public async Task<Result<AutenticacionRespuestaDto>> Registro(UsuarioCrearDto usuarioCrear)
        {
            var emailNormalizado = usuarioCrear.Email.Trim().ToLower();
            var usuarioExiste = await _usuarioRepository.ObtenerUsuarioPorEmail(emailNormalizado);

            if(usuarioExiste != null)
            {
                return Result<AutenticacionRespuestaDto>.Failure($"No se pudo crear usuario con los datos proporcionados");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(usuarioCrear.Password);

            var usuarioModel = new Usuario
            {
                Email = emailNormalizado,
                Nombre = usuarioCrear.Nombre,
                PasswordHash = passwordHash,
                Rol = UsuarioRol.User
            };

            var usuarioCreado = await _autenticacionRepository.Registro(usuarioModel);

            var usuarioCreadoDto = new UsuarioDto
            {
                Id = usuarioCreado.Id,
                Email = usuarioCreado.Email,
                Nombre = usuarioCreado.Nombre,
                FechaCreacion = usuarioCreado.FechaCreacion,
                Rol = usuarioCreado.Rol
            };

            var token = _jwtService.GenerarToken(usuarioModel);

            return Result<AutenticacionRespuestaDto>.Success(new AutenticacionRespuestaDto
            {
                Usuario = usuarioCreadoDto,
                Token = token,
                TiempoExpiracionMinutos = _configuration.GetValue<int>("Jwt:AccessTokenMinutes")
            });
        }
        public async Task<Result<AutenticacionRespuestaDto>> Login(LoginDto loginDto)
        {
            var emailNormalizado = loginDto.Email.Trim().ToLower();
            var usuarioExiste = await _usuarioRepository.ObtenerUsuarioPorEmail(emailNormalizado);

            if (usuarioExiste == null)
            {
                return Result<AutenticacionRespuestaDto>.Failure("Credenciales invalidas");
            }

            var esValido = BCrypt.Net.BCrypt.Verify(loginDto.Password, usuarioExiste.PasswordHash);

            if(!esValido)
            {
                return Result<AutenticacionRespuestaDto>.Failure("Credenciales invalidas");
            }

            var usuarioDto = new UsuarioDto
            {
                Id = usuarioExiste.Id,
                Email = usuarioExiste.Email,
                Nombre = usuarioExiste.Nombre,
                FechaCreacion = usuarioExiste.FechaCreacion,
                Rol = usuarioExiste.Rol
            };

            var token = _jwtService.GenerarToken(usuarioExiste);

            return Result<AutenticacionRespuestaDto>.Success(new AutenticacionRespuestaDto
            {
                Usuario = usuarioDto,
                Token = token,
                TiempoExpiracionMinutos = _configuration.GetValue<int>("Jwt:AccessTokenMinutes")
            });
        }
    }
}
