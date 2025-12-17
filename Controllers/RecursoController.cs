using API_de_Reservas.DTOs.RecursoDtoCarpeta;
using API_de_Reservas.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_de_Reservas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecursoController : ControllerBase
    {
        private readonly IRecursoService _recursoService;

        public RecursoController(IRecursoService recursoService)
        {
            _recursoService = recursoService;
        }


        [HttpPost("crear-recurso")]
        public async Task<IActionResult> CrearRecurso(RecursoCrearDto recursoCrearDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    error = ModelState
                });
            }

            var recursoCreado = await _recursoService.CrearRecurso(recursoCrearDto);

            if(recursoCreado.IsFailure)
            {
                return BadRequest(new
                {
                    success = true,
                    error = recursoCreado.Error
                });
            }

            return Ok(new
            {
                success = true,
                valor = recursoCreado.Value
            });
        }
    }
}
