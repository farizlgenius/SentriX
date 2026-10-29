using Core.Contract.DTOs.Area;
using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;

namespace Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AreaController(IArea area) : ControllerBase
{
      [HttpGet("pagination")]
      public async Task<IActionResult> GetPaginationAsync([FromQuery] PaginationParams param)
      {
            var res = await area.GetPaginationAsync(param);
            return Ok(res);
      }

      [HttpGet("{guid}")]
      public async Task<IActionResult> GetAsync(Guid guid)
      {
            var res = await area.GetByGuidAsync(guid);
            return Ok(res);
      }

      [HttpGet("status/{guid}")]
      public async Task<IActionResult> GetStatusAsync(Guid guid)
      {
            var res = await area.GetStatusAsync(guid);
            return Ok(res);
      }

      [HttpPost]
      public async Task<IActionResult> CreateAsync([FromBody]CreateAreaDto dto)
      {
            var res = await area.CreateAsync(dto);
            return Ok(res);
      }

      [HttpPut]
      public async Task<IActionResult> UpdateAsync([FromBody]UpdateAreaDto dto)
      {
            var res = await area.UpdateAsync(dto);
            return Ok(res);
      }

      [HttpDelete("{guid}")]
      public async Task<IActionResult> DeleteAsync(Guid guid)
      {
            var res = await area.DeleteByGuidAsync(guid);
            return Ok(res);
      }

      [HttpDelete("range")]
      public async Task<IActionResult> DeleteRangeAsync(IEnumerable<Guid> guids)
      {
            var res = await area.DeleteListAsync(guids);
            return Ok(res);
      }

      [HttpPatch("enabled/{guid}")]
      public async Task<IActionResult> EnableAsync(Guid guid)
      {
            var res = await area.EnabledAsync(guid);
            return Ok(res);
      }

      [HttpPatch("disabled/{guid}")]
      public async Task<IActionResult> DisabledAsync(Guid guid)
      {
            var res = await area.DisabledAsync(guid);
            return Ok(res);
      }
}