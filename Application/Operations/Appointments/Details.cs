using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Operations.Appointments
{
    public class Details
    {
        public class Query : IRequest<Result<AppointmentDto>>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<AppointmentDto>>
        {
            private readonly AppDbContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;
            public Handler(AppDbContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<AppointmentDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                //Obtiene el usuario haciendo el request
                var user = await _context.User.Include(x => x.Role)
                    .FirstOrDefaultAsync(x => x.IdDocument == _userAccessor.GetUserIdDocument());

                AppointmentDto appointment;

                //Obtiene una cita
                if (user.Role.Name == "doctor")
                    appointment = await _context.Appointment.Include(x => x.Patient)
                        .ProjectTo<AppointmentDto>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(r => r.Id == request.Id);

                else
                    appointment = await _context.Appointment.Include(x => x.Patient)
                        .Where(x => x.PatientId == user.Id)
                        .ProjectTo<AppointmentDto>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(r => r.Id == request.Id);


                if (appointment == null) return Result<AppointmentDto>.NotFound($"No tiene una cita registrada con el número {request.Id}");

                return Result<AppointmentDto>.Success(appointment);
            }
        }
    }
}