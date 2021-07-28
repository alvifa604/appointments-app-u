using System.Threading.Tasks;
using Application.Operations.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class ServicesController : BaseApiController
    {
        //Endpoint para obtener la lista de servicios
        [HttpGet]
        public async Task<ActionResult> ListServiceAsync()
        {
            return HandleResult(await Mediator.Send(new List.Query { }));
        }

        //Endpoint para obtener un servicio específico
        [HttpGet("{id}")]
        public async Task<ActionResult> GetServiceAsync(int id)
        {
            return HandleResult(await Mediator.Send(new Details.Query { Id = id }));
        }

        //Endpoint para eliminar un servicio específico
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteServiceAsync(int id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
        }

        //Endpoint para crear un servicio
        [HttpPost]
        public async Task<ActionResult> CreateServiceAsync(ServiceDto service)
        {
            return HandleResult(await Mediator.Send(new Create.Command { Service = service }));
        }

        //Endpoint para editar un servicio específico
        [HttpPut("{id}")]
        public async Task<ActionResult> EditServiceAsync(int id, ServiceDto service)
        {
            service.Id = id;
            return HandleResult(await Mediator.Send(new Edit.Command { Service = service }));
        }
    }
}