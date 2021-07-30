using System.Threading.Tasks;
using Application.Operations.Users;
using Application.Operations.Users.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class UsersController : BaseApiController
    {
        //Endpoint para registrarse
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> RegisterAsync(RegisterDto newUser)
        {
            return HandleResult(await Mediator.Send(new Register.Command { RegisterDto = newUser }));
        }

        //Endpoint para hacer login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync(LoginDto user)
        {
            return HandleResult(await Mediator.Send(new Login.Query { LoginDto = user }));
        }

        [Authorize(Roles = "doctor")]
        [HttpPatch("edit-role")]
        public async Task<ActionResult> EditRoleAsync(EditRole.Command command)
        {
            return HandleResult(await Mediator.Send(command));
        }
    }
}