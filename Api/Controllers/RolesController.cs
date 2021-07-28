using System.Threading.Tasks;
using Application.Operations.Roles;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class RolesController : BaseApiController
    {
        //Endpoint para obtener la lista de roles
        [HttpGet]
        public async Task<ActionResult> ListRolesAsync()
        {
            return HandleResult(await Mediator.Send(new List.Query { }));
        }

        //Endpoint para obtener un rol específico
        [HttpGet("{id}")]
        public async Task<ActionResult> GetRoleAsync(int id)
        {
            return HandleResult(await Mediator.Send(new Details.Query { Id = id }));
        }

        //Endpoint para eliminar un rol específico
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRoleAsync(int id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
        }

        //Endpoint para crear un rol
        [HttpPost]
        public async Task<ActionResult> CreateRoleAsync(RoleDto role)
        {
            return HandleResult(await Mediator.Send(new Create.Command { Role = role }));
        }

        //Endpoint para editar un rol específico
        [HttpPut("{id}")]
        public async Task<ActionResult> EditRoleAsync(int id, RoleDto role)
        {
            role.Id = id;
            return HandleResult(await Mediator.Send(new Edit.Command { Role = role }));
        }
    }
}