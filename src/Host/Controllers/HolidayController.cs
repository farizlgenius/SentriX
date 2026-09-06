using Core.Contract.DTOs.Time;
using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;

namespace Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HolidayController(IHoliday holiday) : ControllerBase
{
  // Holiday
  [HttpGet("pagination")]
  public async Task<IActionResult> GetHolidayPaginationAsync([FromQuery] PaginationParams param)
  {
    var res = await holiday.GetPaginationAsync(param);
    return Ok(res);
  }

  [HttpPost]
  public async Task<IActionResult> CreateHolidayAsync([FromBody] CreateHolidayDto dto)
  {
    var res = await holiday.CreateAsync(dto);
    return Ok(res);
  }

  [HttpDelete("{guid}")]
  public async Task<IActionResult> DeleteHolidayAsync(Guid guid)
  {
    var res = await holiday.DeleteByGuidAsync(guid);
    return Ok(res);
  }

  [HttpPut]
  public async Task<IActionResult> UpdateHolidayAsync([FromBody] UpdateHolidayDto dto)
  {
    var res = await holiday.UpdateAsync(dto);
    return Ok(res);
  }



}