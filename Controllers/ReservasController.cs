using API_de_Reservas.DTOs.ReservaDtoCarpeta;
using API_de_Reservas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API_de_Reservas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservasController : ControllerBase
    {
        private readonly IReservasService _reservaService;

        public ReservasController(IReservasService reservaService)
        {
            _reservaService = reservaService;
        }

        [HttpPost("crear-reserva")]
        public async Task<IActionResult> CrearReserva([FromBody] ReservaCrearDto reservaCrearDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    error = ModelState
                });
            }

            var reservaCreada = await _reservaService.CrearReserva(reservaCrearDto);

            if(reservaCreada.IsFailure)
            {
                return BadRequest(new
                {
                    success = false,
                    error = reservaCreada.Error
                });
            }

            return Ok(new
            {
                success = true,
                valor = reservaCreada.Value
            });
        }

        [Authorize]
        [HttpPut("cancelar-reserva/{reservaId}")]
        public async Task<IActionResult> CancelarReserva(int reservaId)
        {
            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(!int.TryParse(usuarioIdClaim, out int usuarioId)) {
                return BadRequest(new
                {
                    success = false,
                    error = "Su userId debe de ser un numero"
                });
            }

            var rol = User.FindFirst(ClaimTypes.Role)?.Value;

            if(reservaId <= 0)
            {
                return BadRequest(new
                {
                    success = false,
                    error = "Su reservaId no puede ser menor o igual a 0"
                });
            }

            var reservaCancelada = await _reservaService.CancelarReserva(reservaId,usuarioId,rol);

            if (reservaCancelada.IsFailure) {
                return BadRequest(new
                {
                    success = false,
                    error = reservaCancelada.Error
                });
            }

            return Ok(new
            {
                success = true,
                valor = reservaCancelada.Value
            });
        }

        [Authorize]
        [HttpGet("obtener-reservas-usuario/{reservaUsuarioId}")]
        public async Task<IActionResult> ObtenerReservasPorUsuario(int reservaUsuarioId)
        {
            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(!int.TryParse(usuarioIdClaim, out int usuarioId))
            {
                return BadRequest(new
                {
                    success = false,
                    error = "Su usuario Id debe de ser un numero"
                });
            }

            var rol = User.FindFirst(ClaimTypes.Role)?.Value;

            var reservasPorUsuario = await _reservaService.ObtenerReservasPorUsuario(reservaUsuarioId,usuarioId, rol);

            if(reservasPorUsuario.IsFailure)
            {
                return BadRequest(new
                {
                    success = false,
                    error = reservasPorUsuario.Error
                });
            }

            return Ok(new
            {
                success = true,
                valor = reservasPorUsuario.Value
            });
        }
    }
}
