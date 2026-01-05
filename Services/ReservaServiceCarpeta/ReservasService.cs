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

        //Usuarios
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
        public async Task<Result> CancelarReservaUsuarioAsync(int reservaId, int usuarioId)
        {
            if(reservaId <= 0)
            {
                return Result.Failure("Su reservaId no puede ser menor o igual a 0");
            }

            var reservaExiste = await _reservaRepository.ObtenerReservaPorIdAsync(reservaId);

            if(reservaExiste == null)
            {
                return Result.Failure($"Su reserva con id = {reservaId} no existe");
            }

            if(reservaExiste.UsuarioId != usuarioId)
            {
                return Result.Failure("No puedes cancelar una reserva que no es tuya");
            }


            if(reservaExiste.Estado == EstadoReserva.Cancelada)
            {
                return Result.Failure($"Su reserva con id = {reservaId} ya esta cancelada");
            }

            reservaExiste.Estado = EstadoReserva.Cancelada;

            await _unidadDeTrabajo.GuardarCambiosAsync();

            return Result.Success();
        }
        public async Task<Result<List<ReservaDto>>> ObtenerReservasPorUsuarioIdAsync(int usuarioId)
        {
            var reservas = await _reservaRepository.ObtenerReservasPorUsuarioAsync(usuarioId);

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



        //admin
        public async Task<Result> CancelarReservaAdminAsync(int reservaId)
        {
            if (reservaId <= 0)
            {
                return Result.Failure("Su reservaId no puede ser menor o igual a 0");
            }

            var reservaExiste = await _reservaRepository.ObtenerReservaPorIdAsync(reservaId);

            if (reservaExiste == null)
            {
                return Result.Failure($"Su reserva con id = {reservaId} no existe");
            }

            if (reservaExiste.Estado == EstadoReserva.Cancelada)
            {
                return Result.Failure($"Su reserva con id = {reservaId} ya esta cancelada");
            }

            reservaExiste.Estado = EstadoReserva.Cancelada;

            await _unidadDeTrabajo.GuardarCambiosAsync();


            return Result.Success();
        }
        public async Task<Result<List<ReservaDto>>> ObtenerReservasPorUsuarioIdAdminAsync(int usuarioId)
        {
            if(usuarioId <= 0)
            {
                return Result<List<ReservaDto>>.Failure($"Su usuarioId no puede ser menor o igual a 0");
            }

            var usuario = await _usuarioRepository.ObtenerUsuarioPorId(usuarioId);

            if(usuario == null)
            {
                return Result<List<ReservaDto>>.Failure($"Su usuario con id = {usuarioId} no existe");
            }

            var reservas = await _reservaRepository.ObtenerReservasPorUsuarioAsync(usuarioId);

            var reservasDto = reservas.Select(r => new ReservaDto
            {
                Id = r.Id,
                UsuarioId = r.UsuarioId,
                Estado = r.Estado,
                FechaCreacion = r.FechaCreacion,
                FechaFinal = r.FechaFinal,
                FechaInicio = r.FechaInicio,
                RecursoId = r.RecursoId
            }).ToList();

            return Result<List<ReservaDto>>.Success(reservasDto);
        }
        public async Task<Result<List<ReservaDto>>> ObtenerReservasPorRecursoAdminAsync(int recursoId)
        {
            if(recursoId <= 0)
            {
                return Result<List<ReservaDto>>.Failure("Su recursoId no puede ser menor o igual a 0");
            }

            var recursoExiste = await _recursoRepository.ObtenerRecursoPorIdAsync(recursoId);

            if(recursoExiste == null)
            {
                return Result<List<ReservaDto>>.Failure($"Su recurso con id = {recursoId} no existe");
            }

            var reservasPorRecurso = await _reservaRepository.ObtenerReservasPorRecursoAsync(recursoId);

            var reservasDto = reservasPorRecurso.Select(r => new ReservaDto { 
                RecursoId=r.RecursoId,
                Estado  =r.Estado,
                FechaCreacion=r.FechaCreacion,
                FechaFinal = r.FechaFinal,  
                FechaInicio=r.FechaInicio,
                Id =r.Id,
                UsuarioId = r.UsuarioId
            }).ToList();

            return Result<List<ReservaDto>>.Success(reservasDto);
        }
        public async Task<Result<List<ReservaDto>>> ObtenerTodasLasReservasAdminAsync(int page, int pageSize, DateTime? fechaInicio, DateTime? fechaFinal)
        {
            if (page <= 0)
                return Result<List<ReservaDto>>.Failure("La página debe ser mayor a 0");

            if (pageSize <= 0 || pageSize > 50)
                return Result<List<ReservaDto>>.Failure("pageSize inválido");

            if (fechaFinal < fechaInicio)
            {
                return Result<List<ReservaDto>>.Failure("El rango de fechas es inválido");
            }

            var reservas = await _reservaRepository.ObtenerTodasLasReservasAsync(page,pageSize,fechaInicio,fechaFinal);

            var reservasDto = reservas.Select(r => new ReservaDto
            {
                RecursoId = r.RecursoId,
                Estado = r.Estado,
                FechaCreacion = r.FechaCreacion,
                FechaFinal = r.FechaFinal,
                FechaInicio = r.FechaInicio,
                Id = r.Id,
                UsuarioId = r.UsuarioId
            }).ToList();

            return Result<List<ReservaDto>>.Success(reservasDto);
        }
    }
}
