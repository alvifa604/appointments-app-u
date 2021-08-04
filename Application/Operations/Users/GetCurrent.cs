using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Domain;

namespace Application.Operations.Users.Dtos
{
    public class GetCurrent
    {
        public class Query : IRequest<Result<UserDto>> { }

        public class Handler : IRequestHandler<Query, Result<UserDto>>
        {
            private readonly IUserAccessor _userAccessor;
            private readonly AppDbContext _context;
            private readonly ITokenService _tokenService;
            public Handler(AppDbContext context, IUserAccessor userAccessor, ITokenService tokenService)
            {
                _tokenService = tokenService;
                _context = context;
                _userAccessor = userAccessor;
            }

            public async Task<Result<UserDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.User.Include(r => r.Role)
                    .FirstOrDefaultAsync(x => x.IdDocument == _userAccessor.GetUserIdDocument());
                if (user == null) return Result<UserDto>.Failure("Error al obtener el usuario");

                return Result<UserDto>.Success(CreateUserDto(user));
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