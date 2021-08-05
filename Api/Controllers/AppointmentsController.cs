using System.Threading.Tasks;
using Application.Operations.Appointments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class AppointmentsController : BaseApiController
    {
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateAppointmentAsync(Create.Command appointment)
        {
            return HandleResult(await Mediator.Send(appointment));
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetAppointmentsAsync()
        {
            return HandleResult(await Mediator.Send(new List.Query { }));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAppointmentAsync(int id)
        {
            return HandleResult(await Mediator.Send(new Details.Query { Id = id }));
        }

        [Authorize]
        [HttpPatch()]
        public async Task<ActionResult> EditAppointmentAsync(Edit.Command command)
        {
            return HandleResult(await Mediator.Send(command));
        }
    }
}