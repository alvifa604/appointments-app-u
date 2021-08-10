using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Operations.Appointments
{
    public class Create
    {
        public class Command : IRequest<Result<AppointmentDto>>
        {
            public string PacientIdDocument { get; set; }
            public int ServiceId { get; set; }
            public DateTime Date { get; set; }
        }
        public class CommandValidor : AbstractValidator<Command>
        {
            public CommandValidor()
            {
                RuleFor(x => x.PacientIdDocument).NotEmpty().WithMessage("La cédula del paciente no puede estar vacía");
                RuleFor(x => x.ServiceId).NotEmpty().WithMessage("El id del servicio no puede ser vacío");
                RuleFor(x => x.Date).NotEmpty().WithMessage("Ingrese la fecha de la cita");
            }
        }

        public class Handler : IRequestHandler<Command, Result<AppointmentDto>>
        {
            private readonly AppDbContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly IMapper _mapper;
            public Handler(AppDbContext context, IUserAccessor userAccessor, IMapper mapper)
            {
                _mapper = mapper;
                _userAccessor = userAccessor;
                _context = context;
            }

            public async Task<Result<AppointmentDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request.Date < DateTime.Now.AddDays(5))
                    return Result<AppointmentDto>.Failure("Las citas deben agendarse al menos cinco días de anticipación");
                

                var user = await _context.User.Include(x => x.Role)
                    .FirstOrDefaultAsync(x => x.IdDocument == _userAccessor.GetUserIdDocument());

                User patient;
                if (user.Role.Name == "doctor")
                {
                    patient = await _context.User.FirstOrDefaultAsync(x => x.IdDocument == request.PacientIdDocument);
                    if (patient == null) return Result<AppointmentDto>.NotFound($"No hay un paciente con el número de cédula {request.PacientIdDocument}");
                }
                else
                {
                    patient = user;
                    if (patient.IdDocument != request.PacientIdDocument)
                    {
                        return Result<AppointmentDto>.Failure("No se puede agendar citas a terceros");
                    }
                }

                //Valida que no tenga una cita para ese dia
                var appointments = _context.Appointment.Where(x => x.PatientId == patient.Id).ToList();
                foreach (var a in appointments)
                    if (a.Date == request.Date)
                        return Result<AppointmentDto>.Failure($"Ya tiene una cita agendada para la fecha {request.Date.ToLocalTime()}");

                //Obtiene el servicio a agendar
                var service = await _context.Service.FirstOrDefaultAsync(x => x.Id == request.ServiceId);
                if (service == null) 
                    return Result<AppointmentDto>.NotFound($"No hay un servicio con el id {request.ServiceId}");

                Appointment appointment = new()
                {
                    Patient = patient,
                    Service = service,
                    Date = request.Date
                };

                _context.Appointment.Add(appointment);
                var result = await _context.SaveChangesAsync() > 0;

                var appointmentDto = _mapper.Map<Appointment, AppointmentDto>(appointment);

                if (result) return Result<AppointmentDto>.Success(appointmentDto);
                return Result<AppointmentDto>.Failure("Error creando la cita");
            }
        }
    }
}