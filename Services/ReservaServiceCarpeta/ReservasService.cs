using API_de_Reservas.DALs;
using API_de_Reservas.DALs.RecursoRepositoryCarpeta;
using API_de_Reservas.DALs.ReservaRepositoryCarpeta;
using API_de_Reservas.DALs.UsuarioRepositoryCarpeta;
using API_de_Reservas.DTOs.ReservaDtoCarpeta;
using API_de_Reservas.Models;
using API_de_Reservas.Models.Enums;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace API_de_Reservas.Services.ReservaServiceCarpeta
{
    public class ReservasService : IReservasService
    {
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;
        private readonly IReservasRepository _reservaRepository;
        private readonly IRecursoRepository _recursoRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public ReservasService(IReservasRepository reservaRepository, IRecursoRepository recursoRepository, IUsuarioRepository usuarioRepository,IUnidadDeTrabajo unidadDeTrabajo) { 
            _unidadDeTrabajo = unidadDeTrabajo;
            _reservaRepository = reservaRepository;
            _recursoRepository = recursoRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Result<ReservaDto>> CrearReservaAsync(ReservaCrearDto reservaCrearDto, int usuarioId)
        {
            var fechaActual = DateTime.UtcNow;
            var fechaInicioReservaCrearDto = reservaCrearDto.FechaInicio;
            var fechaFinalReservaCrearDto = reservaCrearDto.FechaFinal;
            var tolerancia = TimeSpan.FromMinutes(1);
            var fechaMinimaPermitida = fechaActual.Subtract(tolerancia);

            if (fechaInicioReservaCrearDto < fechaMinimaPermitida)
            {
                return Result<ReservaDto>.Failure("No se pudo crear la reserva por que la fecha de inicio es pasada a la fecha actual");
            }

            if (fechaFinalReservaCrearDto < fechaMinimaPermitida)
            {
                return Result<ReservaDto>.Failure("No se pudo crear la reserva porque la fecha final es pasada a la fecha actual");
            }

            if (fechaFinalReservaCrearDto < fechaInicioReservaCrearDto)
            {
                return Result<ReservaDto>.Failure("No se pudo crear la reserva porque la fecha final es anterior a la fecha de inicio");
            }

            var recurso = await _recursoRepository.ObtenerRecursoPorIdAsync(reservaCrearDto.RecursoId);
            if(recurso == null)
            {
                return Result<ReservaDto>.Failure($"El recurso con id = {reservaCrearDto.RecursoId} no existe");
            }

            foreach(var reserva in recurso.Reservas)
            {
                if(reserva.Estado == EstadoReserva.Cancelada)
                {
                    continue;
                }

                if (reservaCrearDto.FechaInicio <= reserva.FechaFinal && reservaCrearDto.FechaFinal >= reserva.FechaInicio)
                {
                    return Result<ReservaDto>.Failure("El recurso ya está reservado en el horario seleccionado");
                } 
            }

            var reservaModel = new Reserva
            {
                UsuarioId = usuarioId,
                RecursoId = reservaCrearDto.RecursoId,
                Estado = EstadoReserva.Activo,
                FechaCreacion = DateTime.UtcNow,
                FechaInicio = reservaCrearDto.FechaInicio,
                FechaFinal = reservaCrearDto.FechaFinal
            };

            var reservaCreada = _reservaRepository.CrearReserva(reservaModel);

            await _unidadDeTrabajo.GuardarCambiosAsync();

            var reservaCreadaDto = new ReservaDto
            {
                Id = reservaCreada.Id,
                Estado = reservaCreada.Estado,
                FechaCreacion = reservaCreada.FechaCreacion,
                FechaInicio = reservaCreada.FechaInicio,
                FechaFinal = reservaCreada.FechaFinal,
                RecursoId = reservaCreada.RecursoId,
                UsuarioId = reservaCreada.UsuarioId
            };

            return Result<ReservaDto>.Success(reservaCreadaDto);
        }

        public async Task<Result<ReservaDto>> CancelarReserva(int reservaId, int usuarioId, string rol)
        {
            var usuarioExiste = await _usuarioRepository.ObtenerUsuarioPorId(usuarioId);

            if(usuarioExiste == null)
            {
                return Result<ReservaDto>.Failure($"Usuario con id = {usuarioId} no existe");
            }

            var reservaExiste = await _reservaRepository.ObtenerReservaPorId(reservaId);

            if(reservaExiste == null)
            {
                return Result<ReservaDto>.Failure($"Su reserva con id = {reservaId} no existe");
            }

            if(reservaExiste.UsuarioId != usuarioId && rol != UsuarioRol.Admin.ToString())
            {
                return Result<ReservaDto>.Failure("No puedes cancelar una reserva que no es tuya");
            }


            if(reservaExiste.Estado == EstadoReserva.Cancelada)
            {
                return Result<ReservaDto>.Failure($"Su reserva con id = {reservaId} ya esta cancelada");
            }

            var reservaCancelada = await _reservaRepository.CancelarReserva(reservaId);

            var reservaCanceladaDto = new ReservaDto
            {
                Id = reservaCancelada.Id,
                Estado = reservaCancelada.Estado,
                FechaCreacion = reservaCancelada.FechaCreacion,
                FechaFinal = reservaCancelada.FechaFinal,
                FechaInicio = reservaCancelada.FechaInicio,
                RecursoId = reservaCancelada.RecursoId,
                UsuarioId = reservaCancelada.UsuarioId,
            };

            return Result<ReservaDto>.Success(reservaCanceladaDto);
        }

        public async Task<Result<List<ReservaDto>>> ObtenerReservasPorUsuario(int reservaUsuarioId, int usuarioId, string rol)
        {
            var usuarioExiste = await _usuarioRepository.ObtenerUsuarioPorId(reservaUsuarioId);

            if(usuarioExiste == null)
            {
                return Result<List<ReservaDto>>.Failure($"Su usuario con id = {usuarioId} no existe");
            }

            if(usuarioExiste.Id != usuarioId && rol != UsuarioRol.Admin.ToString())
            {
                return Result<List<ReservaDto>>.Failure("No puede ver reservas que no son suyas");
            }

            var reservas = await _reservaRepository.ObtenerReservasPorUsuario(reservaUsuarioId);

            var reservasDto = reservas.Select(r => new ReservaDto
            {
                Id = r.Id,
                UsuarioId= r.UsuarioId,
                Estado = r.Estado,
                FechaCreacion = r.FechaCreacion,
                FechaFinal = r.FechaFinal,
                FechaInicio = r.FechaInicio,
                RecursoId = r.RecursoId 
            }).ToList();

            return Result<List<ReservaDto>>.Success(reservasDto);
        }
        public async Task<Result<List<ReservaDto>>> ObtenerReservasPorRecurso(int recursoId)
        {
            var recursoExiste = await _recursoRepository.ObtenerRecursoPorId(recursoId);

            if(recursoExiste == null)
            {
                return Result<List<ReservaDto>>.Failure("Su recurso con id no existe");
            }

            var reservasPorRecurso = await _reservaRepository.ObtenerReservasPorRecurso(recursoId);

            var reservasDto = reservasPorRecurso.Select(r => new ReservaDto { 
                RecursoId=r.Id,
                Estado  =r.Estado,
                FechaCreacion=r.FechaCreacion,
                FechaFinal = r.FechaFinal,  
                FechaInicio=r.FechaInicio,
                Id =r.Id,
                UsuarioId = r.UsuarioId
            }).ToList();

            return Result<List<ReservaDto>>.Success(reservasDto);
        }

    }
}
