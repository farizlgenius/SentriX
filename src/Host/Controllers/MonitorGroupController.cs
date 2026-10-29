using Core.Contract.DTOs.MonitorGroup;
using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;

namespace Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MonitorGroupController(IMonitorGroup group) : ControllerBase
{
      // Monitor Group

      [HttpGet("pagination")]
      public async Task<IActionResult> GetPaginationAsync([FromQuery] PaginationParams param)
      {
            var res = await group.GetPaginationAsync(param);
            return Ok(res);
      }

      [HttpGet("{guid}")]
      public async Task<IActionResult> GetAsync(Guid guid)
      {
            var res = await group.GetByGuidAsync(guid);
            return Ok(res);
      }

      [HttpGet("location/{guid}")]
      public async Task<IActionResult> GetByLocationAsync(Guid guid)
      {
            var res = await group.GetByLocationAsync(guid);
            return Ok(res);
      }

      [HttpPost]
      public async Task<IActionResult> CreateAsync([FromBody]CreateMonitorGroupDto dto)
      {
            var res = await group.CreateAsync(dto);
            return Ok(res);
      }

      [HttpPost("arm/{guid}")]
      public async Task<IActionResult> ArmAsync(Guid guid)
      {
            var res = await group.ArmAsync(guid);
            return Ok(res);
      }

      [HttpPost("disarm/{guid}")]
      public async Task<IActionResult> DisarmAsync(Guid guid)
      {
            var res = await group.DisarmAsync(guid);
            return Ok(res);
      }

      [HttpDelete("range")]
      public async Task<IActionResult> DeleteRangeAsync(IEnumerable<Guid> guids)
      {
            var res = await group.DeleteListAsync(guids);
            return Ok(res);
      }

      [HttpPut]
      public async Task<IActionResult> UpdateAsync([FromBody]UpdateMonitorGroupDto dto)
      {
            var res = await group.UpdateAsync(dto);
            return Ok(res);
      }

      [HttpPost("enabled/{guid}")]
      public async Task<IActionResult> EnabledAsync(Guid guid)
      {
            var res = await group.EnabledAsync(guid);
            return Ok(res);
      }

      [HttpPost("disabled/{guid}")]
      public async Task<IActionResult> DisabledAsync(Guid guid)
      {
            var res = await group.DisabledAsync(guid);
            return Ok(res);
      }

      
}