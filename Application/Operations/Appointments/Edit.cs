using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Operations.Appointments
{
    public class Edit
    {
        public class Command : IRequest<Result<AppointmentDto>>
        {
            public int AppointmentId { get; set; }
            public bool Canceled { get; set; }
            public bool Completed { get; set; }
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
                var user = await _context.User.Include(x => x.Role)
                    .FirstOrDefaultAsync(x => x.IdDocument == _userAccessor.GetUserIdDocument());

                Appointment appointment;

                if (user.Role.Name == "doctor")
                {
                    appointment = _context.Appointment.Include(s => s.Service).FirstOrDefault(x => x.Id == request.AppointmentId);
                    if (appointment == null) return Result<AppointmentDto>.NotFound($"No se encontró la cita con el número {request.AppointmentId}");

                    appointment.IsCancelled = request.Canceled;
                    appointment.IsCompleted = request.Completed;
                }
                else
                {
                    appointment = _context.Appointment.Include(s => s.Service).FirstOrDefault(x => x.Id == request.AppointmentId && x.PatientId == user.Id);
                    if (appointment == null) return Result<AppointmentDto>.NotFound($"No tiene una cita con el número {request.AppointmentId}");
                    appointment.IsCancelled = request.Canceled;
                }

                var result = await _context.SaveChangesAsync();

                var appointmentDto = _mapper.Map<Appointment, AppointmentDto>(appointment);

                //Verifica si se guardaron
                if (result > 0) return Result<AppointmentDto>.Success(appointmentDto);
                if (result == 0) return Result<AppointmentDto>.Failure("No hubo cambios en la base de datos");

                return Result<AppointmentDto>.Failure("Error editando la cita");
            }
        }
    }
}