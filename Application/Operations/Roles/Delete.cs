using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using MediatR;
using Persistence;

namespace Application.Operations.Roles
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int Id { get; set; }
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
                //Obtiene el rol a eliminar y verifica si existe
                var role = await _context.Role.FindAsync(request.Id);
                if(role == null) return Result<Unit>.NotFound($"No existe un rol con el id {request.Id}");

                //Elimina el rol
                _context.Role.Remove(role);
                var success = await _context.SaveChangesAsync() > 0;

                //Notifica si se eliminó
                if(success) return Result<Unit>.Success(Unit.Value);
                return Result<Unit>.Failure("Algo salió mal eliminando de la base de datos");
            }
        }
    }
}