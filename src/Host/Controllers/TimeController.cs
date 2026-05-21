using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;
using Time.Contract.DTOs;
using Time.Contract.Interfaces;

namespace Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TimeController(ITime time) : ControllerBase
{
      // Holiday
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

      [HttpDelete("holiday/{id}")]
      public async Task<IActionResult> DeleteHolidayAsync(int id)
      {
            var res = await time.DeleteHolidayAsync(id);
            return Ok(res);
      }

      [HttpPut]
      public async Task<IActionResult> UpdateHolidayAsync([FromBody]HolidayDto dto)
      {
            var res = await time.UpdateHolidayAsync(dto);
            return Ok(res);
      }


      // Timezone
      [HttpGet("timezone/pagination")]
      public async Task<IActionResult> GetTimezonePaginationAsync([FromQuery] PaginationParams param)
      {
            var res = await time.TimezonePaginationAsync(param);
            return Ok(res);
      }

      [HttpPost("timezone")]
      public async Task<IActionResult> CreateTimezoneAsync([FromBody]CreateTimezoneDto dto)
      {
            var res = await time.CreateTimezoneAsync(dto);
            return Ok(res);
      }

      [HttpPut]
      public async Task<IActionResult> UpdateTimezoneAsync([FromBody]TimezoneDto dto)
      {
            var res = await time.UpdateTimezoneAsync(dto);
            return Ok(res);
      }

      [HttpDelete("{id}")]
      public async Task<IActionResult> DeleteTimezoneAsync(int id)
      {
            var res = await time.DeleteTimezoneAsync(id);
            return Ok(res);
      }

      [HttpGet("timezone/mode")]
      public async Task<IActionResult> GetTimezoneModeAsync([FromQuery]string Type)
      {
            var res = await time.GetTimezoneModeAsync(Type);
            return Ok(res);
      }


}