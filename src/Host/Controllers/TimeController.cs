using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;
using Time.Contract.DTOs;
using Time.Contract.Interfaces;

namespace Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TimeController(ITime time) : ControllerBase
{
      [HttpGet("holiday/pagination")]
      public async Task<IActionResult> GetHolidayPaginationAsync([FromQuery]PaginationParams param)
      {
            var res = await time.HolidayPaginationAsync(param);
            return Ok(res);
      }

      [HttpPost("holiday")]
      public async Task<IActionResult> CreateHolidayAsync([FromBody] CreateHolidayDto dto)
      {
            var res = await time.CreateHolidayAsync(dto);
            return Ok(res);
      }
}