using System.Collections.Generic;
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
    public class List
    {
        public class Query : IRequest<Result<List<RoleDto>>> { }

        public class Handler : IRequestHandler<Query, Result<List<RoleDto>>>
        {
            private readonly AppDbContext _context;
            private readonly IMapper _mapper;
            public Handler(AppDbContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<List<RoleDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                //Obtiene los roles de la base de datos
                var roles = await _context.Role.ProjectTo<RoleDto>(_mapper.ConfigurationProvider).ToListAsync();

                //Verifica que hayan roles
                if (roles.Count == 0) return Result<List<RoleDto>>.Failure("No hay roles registrados");

                return Result<List<RoleDto>>.Success(roles);
            }
        }
    }
}