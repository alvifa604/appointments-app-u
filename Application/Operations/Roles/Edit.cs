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

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly AppDbContext _context;
            public Handler(AppDbContext context)
            {
                _context = context;
            }

            public class CommandValidator : AbstractValidator<RoleDto>
            {
                public CommandValidator()
                {
                    RuleFor(r => r.Id).NotEmpty();
                    RuleFor(r => r.Name).NotEmpty();
                }
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                //Obtener el rol y verificar que exista
                var role = await _context.Role.FindAsync(request.Role.Id);
                if (role == null) return Result<Unit>.NotFound($"No existe un rol con el id {request.Role.Id}");

                //Editar el rol y guarda los cambios
                role.Name = request.Role.Name;
                var success = await _context.SaveChangesAsync() > 0;

                //Verifica si se guardaron
                if (success) return Result<Unit>.Success(Unit.Value);
                return Result<Unit>.Failure("Error guardando en la base de datos");
            }
        }
    }
}