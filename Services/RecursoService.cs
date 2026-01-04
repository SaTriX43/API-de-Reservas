using API_de_Reservas.DALs;
using API_de_Reservas.DALs.RecursoRepositoryCarpeta;
using API_de_Reservas.DTOs.RecursoDtoCarpeta;
using API_de_Reservas.Models;

namespace API_de_Reservas.Services
{
    public class RecursoService : IRecursoService
    {
        private readonly IRecursoRepository _recursoRepository;
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;

        public RecursoService(IRecursoRepository recursoRepository, IUnidadDeTrabajo unidadDeTrabajo)
        {
            _recursoRepository = recursoRepository;
            _unidadDeTrabajo = unidadDeTrabajo;
        }


        public async Task<Result<RecursoDto>> CrearRecurso(RecursoCrearDto recursoCrearDto)
        {
            var recursoNombreNormalizado = recursoCrearDto.Nombre.Trim().ToLower();
            var recursoExisteNombre = await _recursoRepository.ObtenerRecursoPorNombreAsync(recursoNombreNormalizado);

            if(recursoExisteNombre != null)
            {
                return Result<RecursoDto>.Failure($"El recurso con nombre = {recursoNombreNormalizado} ya existe");
            }

            var recursoModel = new Recurso
            {
                Nombre = recursoNombreNormalizado,
                Descripcion = recursoCrearDto.Descripcion,
                Activo = true,
                Tipo = recursoCrearDto.Tipo,
                FechaCreacion = DateTime.UtcNow
            };

            var recursoCreado = _recursoRepository.CrearRecurso(recursoModel);

            await _unidadDeTrabajo.GuardarCambiosAsync();

            var recursoDto = new RecursoDto
            {
                Id = recursoCreado.Id,
                Nombre = recursoCreado.Nombre,
                Activo = recursoCreado.Activo,
                Tipo = recursoCreado.Tipo,
                FechaCreacion = recursoCreado.FechaCreacion,
                Descripcion = recursoCreado.Descripcion
            };

            return Result<RecursoDto>.Success(recursoDto);
        }
    }
}
