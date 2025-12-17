using API_de_Reservas.DTOs.AutenticacionDtoCarpeta;
using API_de_Reservas.DTOs.UsuarioDtoCarpeta;
using API_de_Reservas.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_de_Reservas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacionController : ControllerBase
    {
        private readonly IAutenticacionService _autenticacionService;

        public AutenticacionController(IAutenticacionService autenticacionService)
        {
            _autenticacionService = autenticacionService;
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Registro(UsuarioCrearDto usuarioCrearDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    error = ModelState
                });
            }

            var usuarioCreado = await _autenticacionService.Registro(usuarioCrearDto);

            if(usuarioCreado.IsFailure)
            {
                return BadRequest(new
                {
                    success = false,
                    error = usuarioCreado.Error
                });
            }

            return Ok(new
            {
                success = true,
                usuarioCreado.Value
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    error = ModelState
                });
            }

            var usuarioLogeado= await _autenticacionService.Login(loginDto);

            if (usuarioLogeado.IsFailure)
            {
                return BadRequest(new
                {
                    success = false,
                    error = usuarioLogeado.Error
                });
            }

            return Ok(new
            {
                success = true,
                usuarioLogeado.Value
            });
        }
    }
}
