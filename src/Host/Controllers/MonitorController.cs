using Core.Contract.DTOs.Monitor;
using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;

namespace Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class MonitorController(IMonitor input) : ControllerBase
{
      [HttpGet("pagination")]
      public async Task<IActionResult> GetPaginationAsync([FromQuery] PaginationParams param)
      {
            var res = await input.GetPaginationAsync(param);
            return Ok(res);
      }

      [HttpGet("{guid}")]
      public async Task<IActionResult> GetAsync(Guid guid)
      {
            var res = await input.GetByGuidAsync(guid);
            return Ok(res);
      }

      [HttpGet("option/device/{guid}")]
  public async Task<IActionResult> GetOptionByDeviceAsync(Guid guid)
  {
    var res = await input.GetOptionByDeviceAsync(guid);
    return Ok(res);
  }


      [HttpGet("status/{guid}")]
      public async Task<IActionResult> GetStatusAsync(Guid guid)
      {
            var res = await input.GetStatusAsync(guid);
            return Ok(res);
      }

      [HttpPost("mask/{guid}")]
      public async Task<IActionResult> MaskAsync(Guid guid)
      {
            var res = await input.MaskAsync(guid);
            return Ok(res);
      }

      [HttpPost("unmask/{guid}")]
      public async Task<IActionResult> UnmaskAsync(Guid guid)
      {
            var res = await input.UnmaskAsync(guid);
            return Ok(res);
      }

      [HttpPost]
      public async Task<IActionResult> CreateAsync([FromBody] CreateMonitorDto dto)
      {
            var res = await input.CreateAsync(dto);
            return Ok(res);
      }

      [HttpDelete("{guid}")]
      public async Task<IActionResult> DeleteAsync(Guid guid)
      {
            var res = await input.DeleteByGuidAsync(guid);
            return Ok(res);
      }

      [HttpDelete("range")]
      public async Task<IActionResult> DeleteRangeAsync([FromBody]IEnumerable<Guid> guids)
      {
            var res = await input.DeleteListAsync(guids);
            return Ok(res);
      }

      [HttpPut]
      public async Task<IActionResult> UpdateAsync([FromBody]UpdateMonitorDto dto)
      {
            var res = await input.UpdateAsync(dto);
            return Ok(res);
      }


      
}