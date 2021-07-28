using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Operations.Services
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public ServiceDto Service { get; set; }
        }
        public class CommandValidator : AbstractValidator<ServiceDto>
        {
            public CommandValidator()
            {
                RuleFor(s => s.Name).NotEmpty();
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
                //Obtener el servicio y verificar que exista
                var service = await _context.Service.FindAsync(request.Service.Id);
                if (service == null) return Result<Unit>.NotFound($"No existe un rol con el id {request.Service.Id}");

                //Editar el servicio y guarda los cambios
                service.Name = request.Service.Name;
                var result = await _context.SaveChangesAsync();

                //Verifica si se guardaron
                if (result > 0) return Result<Unit>.Success(Unit.Value);
                if (result == 0) return Result<Unit>.Failure("No hubo cambios en la base de datos");

                return Result<Unit>.Failure("Error guardando en la base de datos");
            }
        }
    }
}