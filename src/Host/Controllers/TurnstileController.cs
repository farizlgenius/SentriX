using System.Security.Claims;
using Core.Contract.DTOs.Turnstile;
using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;

namespace Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TurnstileController(ITurnstile turnstile) : ControllerBase
{
  [HttpGet("pagination")]
  public async Task<IActionResult> GetPaginationAsync([FromQuery] PaginationParams param)
  {
    var res = await turnstile.GetPaginationAsync(param);
    return Ok(res);
  }

  [HttpGet("{guid}")]
  public async Task<IActionResult> GetByGuidAsync(Guid guid)
  {
    var res = await turnstile.GetByGuidAsync(guid);
    return Ok(res);
  }

  [HttpGet("location/{guid}")]
  public async Task<IActionResult> GetByLocationAsync(Guid guid)
  {
    var res = await turnstile.GetByLocationAsync(guid);
    return Ok(res);
  }

  [HttpPost]
  public async Task<IActionResult> CreateAsync([FromBody] CreateTurnstileDto dto)
  {
    
        
    var res = await turnstile.CreateAsync(dto);
    return Ok(res);
  }

  [HttpDelete("{guid}")]
  public async Task<IActionResult> DeleteByGuidAsync(Guid guid)
  {
    

    var res = await turnstile.DeleteByGuidAsync(guid);
    return Ok(res);
  }

  [HttpDelete("list")]
  public async Task<IActionResult> DeleteListAsync([FromBody] IEnumerable<Guid> guids)
  {
    

    var res = await turnstile.DeleteListAsync(guids);
    return Ok(res);
  }

  [HttpPut]
  public async Task<IActionResult> UpdateAsync([FromBody] UpdateTurnstileDto dto)
  {
    

    var res = await turnstile.UpdateAsync(dto);
    return Ok(res);
  }

  [HttpPatch("enable/{guid}")]
  public async Task<IActionResult> EnabledAsync(Guid guid)
  {
    

    var res = await turnstile.EnabledAsync(guid);
    return Ok(res);
  }

  [HttpPatch("disable/{guid}")]
  public async Task<IActionResult> DisabledAsync(Guid guid)
  {
    

    var res = await turnstile.DisabledAsync(guid);
    return Ok(res);
  }


}