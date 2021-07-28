using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Operations.Roles
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public RoleDto Role { get; set; }
        }
        public class CommandValidator : AbstractValidator<RoleDto>
        {
            public CommandValidator()
            {
                RuleFor(r => r.Name).NotEmpty();
            }
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
                //Obtener el rol y verificar que exista
                var role = await _context.Role.FindAsync(request.Role.Id);
                if (role == null) return Result<Unit>.NotFound($"No existe un rol con el id {request.Role.Id}");

                //Editar el rol y guarda los cambios
                role.Name = request.Role.Name;
                var result = await _context.SaveChangesAsync();

                //Verifica si se guardaron
                if (result > 0) return Result<Unit>.Success(Unit.Value);
                if (result == 0) return Result<Unit>.Failure("No hubo cambios en la base de datos");

                return Result<Unit>.Failure("Error guardando en la base de datos");
            }
        }
    }
}