using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Operations.Users
{
    public class EditRole
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string UserId { get; set; }
            public int RoleId { get; set; }
        }
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.UserId).NotEmpty();
                RuleFor(x => x.RoleId).NotEmpty();
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
                var user = await _context.User.FirstOrDefaultAsync(x => x.IdDocument == request.UserId);
                if (user == null) return Result<Unit>.NotFound($"No hay un paciente con el número de cédula {request.UserId}");

                var role = await _context.Role.FindAsync(request.RoleId);
                if (role == null) return Result<Unit>.NotFound($"No hay existe un rol con el id {request.RoleId}");

                user.Role = role;

                var result = await _context.SaveChangesAsync() > 0;
                if (result) return Result<Unit>.Success(Unit.Value);

                return Result<Unit>.Failure("Error cambiando el rol al usuario");
            }
        }
    }
}