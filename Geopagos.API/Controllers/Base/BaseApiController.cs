using Geopagos.Services.Base;
using Microsoft.AspNetCore.Mvc;

namespace Geopagos.API.Controllers.Base
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected IActionResult HandleResponse<T>(ServiceResponse<T> response)
        {
            if (response == null)
                return NotFound();

            if (!response.Status)
                return BadRequest(response.Errors);

            return Ok(response.Data);
        }

        protected IActionResult HandleResponse(ServiceResponse response)
        {
            if (response == null)
                return NotFound();

            if (!response.Status)
                return BadRequest(response.Errors);

            return Ok(response.ReturnValue);
        }
    }
}
