using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Operations.Users
{
    public class GetProfile
    {
        public class Query : IRequest<Result<Dtos.Profile>>
        {
            public string IdDocument { get; set; }
        }
        public class Handler : IRequestHandler<Query, Result<Dtos.Profile>>
        {
            private readonly AppDbContext _context;
            private readonly IMapper _mapper;
            public Handler(AppDbContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<Dtos.Profile>> Handle(Query request, CancellationToken cancellationToken)
            {
                var profile = await _context.User
                    .Include(p => p.Role)
                    .ProjectTo<Dtos.Profile>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(p => p.IdDocument == request.IdDocument);

                if(profile == null) return 
                    Result<Dtos.Profile>.Failure($"No hay un usuario registrado con la cédula {request.IdDocument}");

                return Result<Dtos.Profile>.Success(profile);
            }
        }

    }
}