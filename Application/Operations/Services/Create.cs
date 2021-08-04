using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using AutoMapper;
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
        public class Command : IRequest<Result<ServiceDto>>
        {
            public string Name { get; set; }
        }

        public class CommandValidator : AbstractValidator<ServiceDto>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Name).NotEmpty().WithMessage("Ingrese el nombre del servicio");
            }
        }

        public class Handler : IRequestHandler<Command, Result<ServiceDto>>
        {
            private readonly AppDbContext _context;
            private readonly IMapper _mapper;
            public Handler(AppDbContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<ServiceDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                //Verifica que no exista el servicio
                var service = await _context.Service.FirstOrDefaultAsync(x => x.Name.ToLower() == request.Name.ToLower());
                if (service != null) return Result<ServiceDto>.Failure($"Ya existe el servicio {request.Name.ToLower()}");

                //Crea el servicio
                var newService = new Service() { Name = request.Name.ToLower() };

                //Lo guarda en la base de datos
                _context.Service.Add(newService);
                var success = await _context.SaveChangesAsync() > 0;

                var serviceDto = _mapper.Map<Service, ServiceDto>(newService);

                //Notifica si se guarda o no 
                if (success) return Result<ServiceDto>.Success(serviceDto);
                return Result<ServiceDto>.Failure("Error creando el servicio");
            }
        }
    }
}