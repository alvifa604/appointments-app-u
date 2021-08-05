using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Operations.Users
{
    public class EditRole
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string IdDocument { get; set; }
            public string Role { get; set; }
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
                var user = await _context.User.Include(u => u.Role).FirstOrDefaultAsync(x => x.IdDocument == request.IdDocument);
                if (user == null) return Result<Unit>.NotFound($"No hay un usuario con el número de cédula {request.IdDocument}");

                var role = _context.Role.FirstOrDefault(r => r.Name.ToLower() == request.Role.ToLower());
                if (role == null) return Result<Unit>.NotFound($"No hay existe un rol con el nombre {request.Role.ToLower()}");

                user.Role = role;

                var result = await _context.SaveChangesAsync();
                if (result > 0) return Result<Unit>.Success(Unit.Value);
                if (result == 0) return Result<Unit>.Failure("No hubo cambios en la base de datos");

                return Result<Unit>.Failure("Error cambiando el rol al usuario");
            }
        }
    }
}