using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Operations.Roles
{
    public class Details
    {
        public class Query : IRequest<Result<RoleDto>>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<RoleDto>>
        {
            private readonly AppDbContext _context;
            private readonly IMapper _mapper;
            public Handler(AppDbContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<RoleDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                //Obtiene el rol
                var role = await _context.Role
                    .ProjectTo<RoleDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(r => r.Id == request.Id);
                if (role == null) return Result<RoleDto>.NotFound($"No existe un rol con el id {request.Id}");

                return Result<RoleDto>.Success(role);
            }
        }
    }
}