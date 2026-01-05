using API_de_Reservas.Services.ReservaServiceCarpeta;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API_de_Reservas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminReservasController : ControllerBase
    {
        private readonly IReservasService _reservaService;

        public AdminReservasController(IReservasService reservaService)
        {
            _reservaService = reservaService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> ObtenerTodasLasReservas(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] DateTime? fechaInicio = null,
            [FromQuery] DateTime? fechaFinal = null
            )
        {
            var reservas = await _reservaService.ObtenerTodasLasReservasAdminAsync(page,pageSize,fechaInicio,fechaFinal);

            if (reservas.IsFailure)
            {
                return BadRequest(new
                {
                    success = false,
                    error = reservas.Error
                });
            }

            return Ok(new
            {
                success = true,
                valor = reservas.Value
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("cancelar-reserva/{reservaId}")]
        public async Task<IActionResult> CancelarReserva(int reservaId)
        {
            var reservaCancelada = await _reservaService.CancelarReservaAdminAsync(reservaId);

            if (reservaCancelada.IsFailure)
            {
                return BadRequest(new
                {
                    success = false,
                    error = reservaCancelada.Error
                });
            }

            return NoContent();
        }

        [Authorize]
        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> ObtenerReservasPorUsuario(int usuarioId)
        {
            var reservasPorUsuario = await _reservaService.ObtenerReservasPorUsuarioIdAdminAsync(usuarioId);

            if (reservasPorUsuario.IsFailure)
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
            var reservasPorRecurso = await _reservaService.ObtenerReservasPorRecursoAdminAsync(recursoId);

            if (reservasPorRecurso.IsFailure)
            {
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
