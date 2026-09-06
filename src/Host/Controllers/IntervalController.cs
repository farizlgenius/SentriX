using Core.Contract.DTOs.Time;
using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;

namespace Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IntervalController(IInterval interval) : ControllerBase
{


  [HttpGet("pagination")]
  public async Task<IActionResult> GetTimezonePaginationAsync([FromQuery] PaginationParams param)
  {
    var res = await interval.GetPaginationAsync(param);
    return Ok(res);
  }


  [HttpPost]
  public async Task<IActionResult> CreateTimezoneAsync([FromBody] CreateIntervalDto dto)
  {
    var res = await interval.CreateAsync(dto);
    return Ok(res);
  }

  [HttpPut]
  public async Task<IActionResult> UpdateTimezoneAsync([FromBody] UpdateIntervalDto dto)
  {
    var res = await interval.UpdateAsync(dto);
    return Ok(res);
  }

  [HttpDelete("{guid}")]
  public async Task<IActionResult> DeleteAsync(Guid guid)
  {
    var res = await interval.DeleteByGuidAsync(guid);
    return Ok(res);
  }



}