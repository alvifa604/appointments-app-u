using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Operations.Services
{
    public class List
    {
        public class Query : IRequest<Result<IReadOnlyCollection<ServiceDto>>> { }

        public class Handler : IRequestHandler<Query, Result<IReadOnlyCollection<ServiceDto>>>
        {
            private readonly AppDbContext _context;
            private readonly IMapper _mapper;
            public Handler(AppDbContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<IReadOnlyCollection<ServiceDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                //Obtiene los servicios de la base de datos
                var services = await _context.Service.ProjectTo<ServiceDto>(_mapper.ConfigurationProvider).ToListAsync();

                //Verifica que hayan servicios
                if (services.Count == 0) return Result<IReadOnlyCollection<ServiceDto>>.Failure("No hay servicios registrados");

                return Result<IReadOnlyCollection<ServiceDto>>.Success(services);
            }
        }
    }
}