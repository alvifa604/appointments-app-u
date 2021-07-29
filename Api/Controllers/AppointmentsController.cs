using System.Threading.Tasks;
using Application.Operations.Appointments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize(Roles = "doctor,patient")]
    public class AppointmentsController : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult> CreateAppointmentAsync(Create.Command appointment)
        {
            return HandleResult(await Mediator.Send(appointment));
        }
    }
}