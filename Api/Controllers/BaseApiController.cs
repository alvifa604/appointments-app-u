using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        protected ActionResult HandleResult<T>(Result<T> result)
        {
            //Respuesta 200
            if (result.IsSuccess && result.Found && result.Value != null)
                return Ok(result.Value);

            //Respuesta 404 
            if (result.IsSuccess && !result.Found)
                return NotFound(result.Error);

            //Respuesta 400
            if (!result.IsSuccess && !result.Found)
                return BadRequest(result.Error);

            //Respuesta 401
            if (result.IsSuccess && !result.Found && !result.Authorized)
                return Unauthorized(result.Error);

            return BadRequest(result.Error);
        }
    }
}