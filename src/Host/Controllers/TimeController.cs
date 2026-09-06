using Core.Contract.DTOs.Time;
using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;

namespace Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TimeController(ITime time) : ControllerBase
{


  [HttpGet("pagination")]
  public async Task<IActionResult> GetTimezonePaginationAsync([FromQuery] PaginationParams param)
  {
    var res = await time.GetPaginationAsync(param);
    return Ok(res);
  }


  [HttpPost]
  public async Task<IActionResult> CreateTimezoneAsync([FromBody] CreateTimeZoneDto dto)
  {
    var res = await time.CreateAsync(dto);
    return Ok(res);
  }

  [HttpPut]
  public async Task<IActionResult> UpdateTimezoneAsync([FromBody] UpdateTimeZoneDto dto)
  {
    var res = await time.UpdateAsync(dto);
    return Ok(res);
  }

  [HttpDelete("{guid}")]
  public async Task<IActionResult> DeleteTimezoneAsync(Guid guid)
  {
    var res = await time.DeleteByGuidAsync(guid);
    return Ok(res);
  }



}