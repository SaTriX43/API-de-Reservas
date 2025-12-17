using API_de_Reservas.DALs;
using API_de_Reservas.DTOs.ReservaDtoCarpeta;
using API_de_Reservas.Models;
using API_de_Reservas.Models.Enums;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace API_de_Reservas.Services
{
    public class ReservasService : IReservasService
    {
        private readonly IReservasRepository _reservaRepository;
        private readonly IRecursoRepository _recursoRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public ReservasService(IReservasRepository reservaRepository, IRecursoRepository recursoRepository, IUsuarioRepository usuarioRepository) { 
            _reservaRepository = reservaRepository;
            _recursoRepository = recursoRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Result<ReservaDto>> CrearReserva(ReservaCrearDto reservaCrearDto)
        {
            var usuarioExiste = await _usuarioRepository.ObtenerUsuarioPorId(reservaCrearDto.UsuarioId);

            if (usuarioExiste == null) {
                return Result<ReservaDto>.Failure($"Su usuario con id = {reservaCrearDto.UsuarioId} no existe");
            }

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

            var recurso = await _recursoRepository.ObtenerRecursoPorId(reservaCrearDto.RecursoId);
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
                UsuarioId = reservaCrearDto.UsuarioId,
                RecursoId = reservaCrearDto.RecursoId,
                Estado = EstadoReserva.Activo,
                FechaCreacion = DateTime.UtcNow,
                FechaInicio = reservaCrearDto.FechaInicio,
                FechaFinal = reservaCrearDto.FechaFinal
            };

            var reservaCreada = await _reservaRepository.CrearReserva(reservaModel);

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
    }
}
