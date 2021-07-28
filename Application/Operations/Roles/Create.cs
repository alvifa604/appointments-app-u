using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Operations.Roles
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public RoleDto Role { get; set; }
        }

        public class CommandValidator : AbstractValidator<RoleDto>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Name).NotEmpty().WithMessage("Ingrese el nombre del rol");
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
                //Verifica que no exista el rol
                var role = await _context.Role.FirstOrDefaultAsync(x => x.Name == request.Role.Name);
                if (role != null) return Result<Unit>.Failure($"Ya existe el rol {request.Role.Name}");

                //Crea el rol
                var newRole = new Role() { Name = request.Role.Name };

                //Lo guarda en la base de datos
                _context.Role.Add(newRole);
                var success = await _context.SaveChangesAsync() > 0;

                //Notifica si se guarda o no 
                if (success) return Result<Unit>.Success(Unit.Value);
                return Result<Unit>.Failure("Error creando el rol");
            }
        }
    }
}