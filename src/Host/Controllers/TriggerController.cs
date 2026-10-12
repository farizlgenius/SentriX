


using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TriggerController(ITrigger trigger) : ControllerBase
{
      [HttpGet("source")]
      public async Task<IActionResult> GetSourceAsync()
      {
            var res =  await trigger.GetSourceAsync();
            return Ok(res);
      }

      [HttpGet("type/{source}")]
      public async Task<IActionResult> GetTypeAsync(Guid source)
      {
            var res = await trigger.GetTypeAsync(source);
            return Ok(res);
      }

      [HttpGet("code/{type}")]
      public async Task<IActionResult> GetCodeAsync(Guid type)
      {
            var res = await trigger.GetCodeAsync(type);
            return Ok(res);
      }
}