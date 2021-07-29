using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Operations.Appointments
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
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
                RuleFor(x => x.Date).NotEmpty().WithMessage("Ingrese la fecha de la cita")
                    .GreaterThan(DateTime.Now).WithMessage("La fecha de la cita debe ser superior a la fecha de hoy");
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly AppDbContext _context;
            public Handler(AppDbContext context)
            {
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var patient = await _context.User.FirstOrDefaultAsync(x => x.IdDocument == request.PacientIdDocument);
                if (patient == null) return Result<Unit>.NotFound($"No hay un paciente con el número de cédula {request.PacientIdDocument}");

                var service = await _context.Service.FirstOrDefaultAsync(x => x.Id == request.ServiceId);
                if (service == null) return Result<Unit>.NotFound($"No hay un servicio con el id {request.ServiceId}");

                Appointment appointment = new()
                {
                    Patient = patient,
                    Service = service,
                    Date = request.Date
                };

                _context.Appointment.Add(appointment);
                var result = await _context.SaveChangesAsync() > 0;

                if (result) return Result<Unit>.Success(Unit.Value);
                return Result<Unit>.Failure("Error creando la cita");
            }
        }
    }
}