using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Operations.Services
{
    /// <summary>
    /// Se encarga de la creación de servicios
    /// </summary>
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public ServiceDto Service { get; set; }
        }

        public class CommandValidator : AbstractValidator<ServiceDto>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Name).NotEmpty().WithMessage("Ingrese el nombre del servicio");
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
                //Verifica que no exista el servicio
                var service = await _context.Service.FirstOrDefaultAsync(x => x.Name == request.Service.Name);
                if (service != null) return Result<Unit>.Failure($"Ya existe el servicio {request.Service.Name}");
                
                //Crea el servicio
                var newService = new Service() { Name = request.Service.Name.ToLower() };

                //Lo guarda en la base de datos
                _context.Service.Add(newService);
                var success = await _context.SaveChangesAsync() > 0;

                //Notifica si se guarda o no 
                if (success) return Result<Unit>.Success(Unit.Value);
                return Result<Unit>.Failure("Error creando el servicio");
            }
        }
    }
}