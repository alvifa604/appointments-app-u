using System.Threading.Tasks;
using Application.Operations.Users;
using Application.Operations.Users.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class UsersController : BaseApiController
    {
        //Endpoint para obtener la lista de roles
        /* [HttpGet]
        public async Task<ActionResult> ListRolesAsync()
        {
            return HandleResult(await Mediator.Send(new List.Query { }));
        } */

        //Endpoint para obtener un rol específico
        /* [HttpGet("{id}")]
        public async Task<ActionResult> GetRoleAsync(int id)
        {
            return HandleResult(await Mediator.Send(new Details.Query { Id = id }));
        } */

        //Endpoint para eliminar un rol específico
        /* [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRoleAsync(int id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
        } */

        //Endpoint para registrarse
        [HttpPost("register")]
        public async Task<ActionResult> RegisterAsync(RegisterDto newUser)
        {
            return HandleResult(await Mediator.Send(new Register.Command { RegisterDto = newUser }));
        }

        //Endpoint para hacer login
        [HttpPost("login")]
        public async Task<ActionResult> CreateRoleAsync(LoginDto user)
        {
            return HandleResult(await Mediator.Send(new Login.Query { LoginDto = user }));
        }

        //Endpoint para editar un rol específico
        /* [HttpPut("{id}")]
        public async Task<ActionResult> EditRoleAsync(int id, RoleDto role)
        {
            role.Id = id;
            return HandleResult(await Mediator.Send(new Edit.Command { Role = role }));
        } */
    }
}