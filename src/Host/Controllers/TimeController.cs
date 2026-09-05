using Core.Contract.DTOs.Time;
using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;

namespace Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TimeController(ITime time, IHoliday holiday) : ControllerBase
{
  // Holiday
  [HttpGet("holiday/pagination")]
  public async Task<IActionResult> GetHolidayPaginationAsync([FromQuery] PaginationParams param)
  {
    var res = await holiday.GetPaginationAsync(param);
    return Ok(res);
  }

  [HttpPost("holiday")]
  public async Task<IActionResult> CreateHolidayAsync([FromBody] CreateHolidayDto dto)
  {
    var res = await holiday.CreateAsync(dto);
    return Ok(res);
  }

  [HttpDelete("holiday/{guid}")]
  public async Task<IActionResult> DeleteHolidayAsync(Guid guid)
  {
    var res = await holiday.DeleteByGuidAsync(guid);
    return Ok(res);
  }

  [HttpPut("holiday")]
  public async Task<IActionResult> UpdateHolidayAsync([FromBody] UpdateHolidayDto dto)
  {
    var res = await holiday.UpdateAsync(dto);
    return Ok(res);
  }


  // Timezone
  // [HttpGet("timezone/pagination")]
  // public async Task<IActionResult> GetTimezonePaginationAsync([FromQuery] PaginationParams param)
  // {
  //   var res = await time.TimezonePaginationAsync(param);
  //   return Ok(res);
  // }

  // [HttpGet("timezone/option/{locationId}")]
  // public async Task<IActionResult> GetTimezoneOptionByLocationIdAsync(int locationId)
  // {
  //   var res = await time.GetTimezoneOptionByLocationIdAsync(locationId);
  //   return Ok(res);
  // }

  // [HttpPost("timezone")]
  // public async Task<IActionResult> CreateTimezoneAsync([FromBody] CreateTimezoneDto dto)
  // {
  //   var res = await time.CreateTimezoneAsync(dto);
  //   return Ok(res);
  // }

  // [HttpPut]
  // public async Task<IActionResult> UpdateTimezoneAsync([FromBody] TimeZoneDto dto)
  // {
  //   var res = await time.UpdateTimezoneAsync(dto);
  //   return Ok(res);
  // }

  // [HttpDelete("timezone/{guid}")]
  // public async Task<IActionResult> DeleteTimezoneAsync(Guid guid)
  // {
  //   var res = await time.DeleteTimeZoneByGuidAsync(guid);
  //   return Ok(res);
  // }


}