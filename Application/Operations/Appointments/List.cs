using System.Collections.Generic;
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
    public class List
    {
        public class Query : IRequest<Result<IReadOnlyCollection<AppointmentDto>>> { }

        public class Handler : IRequestHandler<Query, Result<IReadOnlyCollection<AppointmentDto>>>
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

            public async Task<Result<IReadOnlyCollection<AppointmentDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.User.Include(x => x.Role)
                    .FirstOrDefaultAsync(x => x.IdDocument == _userAccessor.GetUserIdDocument());

                List<AppointmentDto> appointments = new();
                //Si el usuario posee el rol doctor, le muesta todas las citas
                if (user.Role.Name == "doctor")
                    appointments = await _context.Appointment
                        .ProjectTo<AppointmentDto>(_mapper.ConfigurationProvider).ToListAsync();
                
                //Si el usuario es paciente, sólo le muesta sus citas
                else
                    appointments = await _context.Appointment.Where(x => x.PatientId == user.Id)
                            .ProjectTo<AppointmentDto>(_mapper.ConfigurationProvider).ToListAsync();

                if (appointments.Count == 0) return Result<IReadOnlyCollection<AppointmentDto>>.NotFound("No hay citas registradas");
                return Result<IReadOnlyCollection<AppointmentDto>>.Success(appointments);
            }
        }
    }
}