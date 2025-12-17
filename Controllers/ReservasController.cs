using API_de_Reservas.DTOs.ReservaDtoCarpeta;
using API_de_Reservas.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
