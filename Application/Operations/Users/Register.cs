using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Application.Operations.Users.Dtos;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Operations.Users
{
    public class Register
    {
        public class Command : IRequest<Result<UserDto>>
        {
            public RegisterDto RegisterDto { get; set; }
        }

        public class CommandValidator : AbstractValidator<RegisterDto>
        {
            public CommandValidator()
            {
                RuleFor(r => r.IdDocument).NotEmpty().WithMessage("La cédula o DIMEX es obligatoria");
                RuleFor(r => r.IdDocument).Length(9, 12).WithMessage("La longitud debe ser entre 9 y 12 caracteres");
                RuleFor(r => r.IdDocument).Matches("^[0-9]+$").WithMessage("La cédula sólo acepta dígitos");

                RuleFor(r => r.Name).NotEmpty().WithMessage("El nombre es obligatorio").MaximumLength(50);
                RuleFor(r => r.FirstLastname).NotEmpty().WithMessage("El primer apellido es obligatorio").MaximumLength(30);
                RuleFor(r => r.SecondLastname).NotEmpty().WithMessage("El segundo apellido es obligatorio").MaximumLength(30);

                RuleFor(r => r.Email).EmailAddress().NotEmpty().WithMessage("El correo es obligatorio").MaximumLength(30);

                RuleFor(r => r.Password).NotEmpty().Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,16}$")
                    .WithMessage("La contraseña debe tener letras, números y entre 8 y 16 caracteres");

                RuleFor(r => r.PhoneNumber).NotEmpty().WithMessage("El número de teléfono es obligatorio");
                RuleFor(r => r.PhoneNumber).Length(8).Matches("^[0-9]+$").WithMessage("El número de teléfono debe ser de 8 dígitos");
            }
        }

        public class Handler : IRequestHandler<Command, Result<UserDto>>
        {
            private readonly AppDbContext _context;
            private readonly ITokenService _tokenService;
            public Handler(AppDbContext context, ITokenService tokenService)
            {
                _tokenService = tokenService;
                _context = context;
            }

            public async Task<Result<UserDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                //Se busca que no exista un usuario con la cedula ingresada
                if (await _context.User.FirstOrDefaultAsync(x => x.IdDocument == request.RegisterDto.IdDocument) != null)
                    return Result<UserDto>.Failure($"Ya existe un usuario con la cédula {request.RegisterDto.IdDocument}");

                //Se revisa que no exista el correo
                if (await _context.User.FirstOrDefaultAsync(x => x.Email == request.RegisterDto.Email.ToLower()) != null)
                    return Result<UserDto>.Failure($"Ya existe un usuario con el correo {request.RegisterDto.Email}");

                //Todo usuario por default es paciente, se extrae el rol para agregarlo al nuevo usuario
                var role = await _context.Role.FirstOrDefaultAsync(x => x.Name == "paciente");

                //Hashea la contraseña 
                var hashedPassword = PasswordService.HashPassword(request.RegisterDto.Password);

                //Se crea un usuario
                User user = new()
                {
                    IdDocument = request.RegisterDto.IdDocument,
                    Name = request.RegisterDto.Name.ToLower(),
                    FirstLastname = request.RegisterDto.FirstLastname.ToLower(),
                    SecondLastname = request.RegisterDto.SecondLastname.ToLower(),
                    BirthDate = request.RegisterDto.BirthDate,
                    Email = request.RegisterDto.Email.ToLower(),
                    PhoneNumber = request.RegisterDto.PhoneNumber,
                    Password = hashedPassword,
                    Role = role
                };

                //Guarda el usuario en la base de datos
                _context.User.Add(user);
                var result = await _context.SaveChangesAsync() > 0;
                if (!result) return Result<UserDto>.Failure("Error registrando al usuario");

                //Crea el dto que se le regresa al cliente
                var userDto = CreateUserDto(user);
                return Result<UserDto>.Success(userDto);
            }

            //Ayuda a crear el Dto del usuario
            private UserDto CreateUserDto(User user)
            {
                return new UserDto
                {
                    Id = user.Id,
                    IdDocument = user.IdDocument,
                    Name = CapitalizeFirstLettter(user.Name),
                    FirstLastname = CapitalizeFirstLettter(user.FirstLastname),
                    SecondLastname = CapitalizeFirstLettter(user.SecondLastname),
                    BirthDate = user.BirthDate,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = user.Role.Name,
                    Token = _tokenService.CreateToken(user)
                };
            }

            private string CapitalizeFirstLettter(string word)
            {
                return char.ToUpper(word[0]) + word.Substring(1);
            }
        }
    }
}