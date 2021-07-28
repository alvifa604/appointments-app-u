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
    public class Login
    {
        public class Query : IRequest<Result<UserDto>>
        {
            public LoginDto LoginDto { get; set; }
        }
        public class QueryValidator : AbstractValidator<LoginDto>
        {
            public QueryValidator()
            {
                RuleFor(x => x.Email).NotEmpty().WithMessage("El correo no debe estar vacío");
                RuleFor(x => x.Password).NotEmpty().WithMessage("La constraseña no debe estar vacía");
            }
        }

        public class Handler : IRequestHandler<Query, Result<UserDto>>
        {
            private readonly AppDbContext _context;
            private readonly ITokenService _tokenService;
            public Handler(AppDbContext context, ITokenService tokenService)
            {
                _tokenService = tokenService;
                _context = context;
            }

            public async Task<Result<UserDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.User
                    .Include(x => x.Role)
                    .FirstOrDefaultAsync(x => x.Email == request.LoginDto.Email.ToLower());
                if (user == null) return Result<UserDto>.NotFound($"No hay un usuario registrado con el correo {request.LoginDto.Email}");

                var isPasswordCorrect = PasswordService.IsPasswordValid(request.LoginDto.Password, user.Password);
                if (!isPasswordCorrect) return Result<UserDto>.Unauthorized("Contraseña incorrecta");

                var userDto = CreateUserDto(user);
                return Result<UserDto>.Success(userDto);
            }

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