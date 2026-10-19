using System.Text.Json;
using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;
using SharedKernel.Enums;


namespace Host.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class EventController(IEvent @event) : ControllerBase
  {
    [HttpGet("pagination")]
    public async Task<IActionResult> GetPagination([FromQuery] PaginationParams param)
    {

      var res = await @event.GetPaginationAsync(param);
      return Ok(res);

    }

    [HttpGet("entity")]
    public async Task<IActionResult> GetCoreEntityAsync()
    {
      var res = await @event.GetCoreEntityAsync();
      return Ok(res);
    }

    [HttpGet("event/{guid}")]
    public async Task<IActionResult> GetCoreEventAsync(Guid guid)
    {
      var res = await @event.GetCoreEventAsync(guid);
      return Ok(res);
    }


    [HttpGet("adapter/pagination")]
    public async Task<IActionResult> GetAdapterPaginationAsync([FromQuery] PaginationParams param)
    {
      var res = await @event.GetAdapterPaginationAsync(param);
      return Ok(res);
    }

    [HttpGet("exception/pagination")]
    public async Task<IActionResult> GetExceptionPaginationAsync([FromQuery] PaginationParams param)
    {
      var res = await @event.GetExceptionPaginationAsync(param);
      return Ok(res);
    }

    [HttpGet("audit/pagination")]
    public async Task<IActionResult> GetAuditPaginationAsync([FromQuery] PaginationParams param)
    {
      var res = await @event.GeAuditPaginationAsync(param);
      return Ok(res);
    }

    // [HttpPost("notifications/dao")]
    // public async Task<IActionResult> DaoAsync([FromBody] JsonElement body)
    // {
    //   Console.WriteLine(body);
    //   await bus.SendAsync(new AmicoNotificationCommand(body));
    //   return Ok();
    // }

    // [HttpGet("capture/{time}")]
    // [Produces("image/png")]
    // public async Task<IActionResult> GetImageAsync(string time)
    // {
    //   var stream = await @event.GetCaptureByTimeAsync(time);
    //   return File(stream, "image/png");
    // }
  }
}
