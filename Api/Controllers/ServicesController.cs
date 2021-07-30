using System.Threading.Tasks;
using Application.Operations.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class ServicesController : BaseApiController
    {
        //Endpoint para obtener la lista de servicios
        [Authorize(Roles = "paciente,doctor")]
        [HttpGet]
        public async Task<ActionResult> ListServiceAsync()
        {
            return HandleResult(await Mediator.Send(new List.Query { }));
        }

        //Endpoint para obtener un servicio específico
        [Authorize(Roles = "paciente,doctor")]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetServiceAsync(int id)
        {
            return HandleResult(await Mediator.Send(new Details.Query { Id = id }));
        }

        //Endpoint para eliminar un servicio específico
        [Authorize(Roles = "doctor")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteServiceAsync(int id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
        }

        //Endpoint para crear un servicio
        [Authorize(Roles = "doctor")]
        [HttpPost]
        public async Task<ActionResult> CreateServiceAsync(ServiceDto service)
        {
            return HandleResult(await Mediator.Send(new Create.Command { Service = service }));
        }

        //Endpoint para editar un servicio específico
        [Authorize(Roles = "doctor")]
        [HttpPut("{id}")]
        public async Task<ActionResult> EditServiceAsync(int id, ServiceDto service)
        {
            service.Id = id;
            return HandleResult(await Mediator.Send(new Edit.Command { Service = service }));
        }
    }
}