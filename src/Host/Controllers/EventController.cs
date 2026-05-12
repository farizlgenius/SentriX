using Events.Contract.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;
using SharedKernel.Exceptions;
using SharedKernel.Helpers;

namespace Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController(IEvent @event) : ControllerBase
    {
        [HttpGet("pagination")]
        public async Task<IActionResult> GetPagination([FromQuery] PaginationParams param)
        {
            var tenants = User.FindFirst("tenants")?.Value ?? "";

            if(!ValidationHelper.ValidateTenants(tenants,param.locationId))
                throw new ForbiddenException(MessageHelper.Location.LocationNotAllow);

            var res = await @event.GetPaginationByLocationIdAsync(param);
            return Ok(res);
            
        }
    }
}
