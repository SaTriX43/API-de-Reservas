using API_de_Reservas.DTOs.ReservaDtoCarpeta;
using API_de_Reservas.Models.Enums;
using API_de_Reservas.Services.ReservaServiceCarpeta;
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

        [Authorize]
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

            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(!int.TryParse(usuarioIdClaim, out int usuarioId)) {
                return BadRequest(new
                {
                    success = false,
                    error = "Su usuarioId debe de ser un numero"
                });
            }

            var reservaCreada = await _reservaService.CrearReservaAsync(reservaCrearDto,usuarioId);

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
        [HttpPatch("cancelar-reserva/{reservaId}")]
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

            var reservaCancelada = await _reservaService.CancelarReservaUsuarioAsync(reservaId,usuarioId);

            if (reservaCancelada.IsFailure) {
                return BadRequest(new
                {
                    success = false,
                    error = reservaCancelada.Error
                });
            }

            return NoContent();
        }

        [Authorize]
        [HttpGet("usuario")]
        public async Task<IActionResult> ObtenerReservasPorUsuario()
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

            var reservasPorUsuario = await _reservaService.ObtenerReservasPorUsuarioIdAsync(usuarioId);

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

        [Authorize(Roles = "Admin")]
        [HttpGet("obtener-reservas-recurso/{recursoId}")]
        public async Task<IActionResult> ObtenerReservasPorRecurso(int recursoId)
        {
            if(recursoId <= 0)
            {
                return BadRequest(new
                {
                    success = false,
                    error = "Su recursoId no puede ser menor o igual a 0"
                });
            }

            var reservasPorRecurso = await _reservaService.ObtenerReservasPorRecurso(recursoId);

            if (reservasPorRecurso.IsFailure) {
                return NotFound(new
                {
                    success = false,
                    error = reservasPorRecurso.Error
                });
            }

            return Ok(new
            {
                success = true,
                valor = reservasPorRecurso.Value
            });
        }
    }
}
