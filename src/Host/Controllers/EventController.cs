using Events.Contract.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;

namespace Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController(IEvent @event) : ControllerBase
    {
        [HttpGet("pagination")]
        public async Task<IActionResult> GetPagination([FromQuery] PaginationParams param)
        {
            var tenant_id = User.FindFirst("tenant_id")?.Value ?? "";
            var res = await @event.GetPaginationByLocationIdAsync(param,tenant_id);
            return Ok(res);
        }
    }
}
