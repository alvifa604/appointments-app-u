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
    public class Details
    {
        public class Query : IRequest<Result<ServiceDto>>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ServiceDto>>
        {
            private readonly AppDbContext _context;
            private readonly IMapper _mapper;
            public Handler(AppDbContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<ServiceDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                //Obtiene el servicio
                var service = await _context.Service
                    .ProjectTo<ServiceDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(r => r.Id == request.Id);

                if (service == null)
                    return Result<ServiceDto>.NotFound($"No existe un servicio con el id {request.Id}");

                return Result<ServiceDto>.Success(service);
            }
        }
    }
}