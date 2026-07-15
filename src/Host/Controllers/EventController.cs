using System.Text.Json;
using Adapter.Abstraction.Command;
using Events.Contract.Interfaces;
using Events.Infrastructure.Persistences.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;
using SharedKernel.Exceptions;
using SharedKernel.Helpers;
using SharedKernel.Messaging;
using SharedKernel.Model;

namespace Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController(Events.Contract.Interfaces.IEvent @event,IMessageBus bus) : ControllerBase
    {
        [HttpGet("pagination")]
        public async Task<IActionResult> GetPagination([FromQuery] PaginationParams param)
        {
            // var tenants = User.FindFirst("tenants")?.Value ?? "";

            // if(!ValidationHelper.ValidateTenants(tenants,param.locationId))
            //     throw new ForbiddenException(MessageHelper.Location.LocationNotAllow);

            var res = await @event.GetPaginationByLocationIdAsync(param);
            return Ok(res);
            
        }

        [HttpGet("command/pagination")]
        public async Task<IActionResult> GetCommandPaginationAsync([FromQuery]PaginationParams param)
        {
            var res = await @event.GetCommandPaginationAsync(param);
            return Ok(res);
        }

        [HttpPost("notifications/dao")]
        public async Task<IActionResult> DaoAsync([FromBody]JsonElement body)
        {
            Console.WriteLine(body);
            await bus.SendAsync(new AmicoNotificationCommand(body));
            return Ok();
        }

        [HttpGet("capture/{time}")]
      [Produces("image/png")]
      public async Task<IActionResult> GetImageAsync(string time)
      {
            var stream = await @event.GetCaptureByTimeAsync(time);
            return File(stream, "image/png");
      }
    }
}
