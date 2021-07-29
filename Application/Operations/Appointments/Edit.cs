using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Operations.Appointments
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int AppointmentId { get; set; }
            public bool Canceled { get; set; }
            public bool Completed { get; set; }
        }
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(r => r.AppointmentId).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly AppDbContext _context;
            private readonly IUserAccessor _userAccessor;
            public Handler(AppDbContext context, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _context = context;
            }


            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.User.Include(x => x.Role)
                    .FirstOrDefaultAsync(x => x.IdDocument == _userAccessor.GetUserIdDocument());

                Appointment appointment;

                if (user.Role.Name == "doctor")
                {
                    appointment = _context.Appointment.Find(request.AppointmentId);
                    if (appointment == null) return Result<Unit>.NotFound($"No se encontró la cita con el número {request.AppointmentId}");

                    appointment.IsCancelled = request.Canceled;
                    appointment.IsCompleted = request.Completed;
                }
                else
                {
                    appointment = _context.Appointment.FirstOrDefault(x => x.Id == request.AppointmentId && x.PatientId == user.Id);
                    if (appointment == null) return Result<Unit>.NotFound($"No tiene una cita con el número {request.AppointmentId}");
                    appointment.IsCancelled = request.Canceled;
                }

                var result = await _context.SaveChangesAsync();

                //Verifica si se guardaron
                if (result > 0) return Result<Unit>.Success(Unit.Value);
                if (result == 0) return Result<Unit>.Failure("No hubo cambios en la base de datos");

                return Result<Unit>.Failure("Error editando la cita");
            }
        }
    }
}