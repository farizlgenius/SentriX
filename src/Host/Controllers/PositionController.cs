using Core.Contract.DTOs.Department;
using Core.Contract.DTOs.Position;
using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;

namespace Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PositionController(IPosition dep) : ControllerBase
{
      [HttpGet("pagination")]
      public async Task<IActionResult> GetPaginationAsync([FromQuery] PaginationParams param)
      {
            var res = await dep.GetPaginationAsync(param);
            return Ok(res);
      }

      [HttpGet("pagination/{guid}")]
      public async Task<IActionResult> GetPaginationByDepartmentGuidAsync(Guid guid, [FromQuery] PaginationParams param)
      {
            var res = await dep.GetPaginationByDepartmentGuidAsync(param, guid);
            return Ok(res);
      }

      [HttpGet("{guid}")]
      public async Task<IActionResult> GetAsync([FromQuery] Guid guid)
      {
            var res = await dep.GetByGuidAsync(guid);
            return Ok(res);
      }

      [HttpPost]
      public async Task<IActionResult> CreateAsync([FromBody] CreatePositionDto dto)
      {
            var res = await dep.CreateAsync(dto);
            return Ok(res);
      }

      [HttpPut]
      public async Task<IActionResult> UpdateAsync([FromBody] UpdatePositionDto dto)
      {
            var res = await dep.UpdateAsync(dto);
            return Ok(res);
      }

      [HttpDelete("{guid}")]
      public async Task<IActionResult> DeleteAsync(Guid guid)
      {
            var res = await dep.DeleteByGuidAsync(guid);
            return Ok(res);
      }

      [HttpDelete("range")]
      public async Task<IActionResult> DeleteRangeAsync([FromBody] IEnumerable<Guid> guids)
      {
            var res = await dep.DeleteListAsync(guids);
            return Ok(res);
      }

      [HttpPatch("enable/{guid}")]
      public async Task<IActionResult> EnableAsync(Guid guid)
      {
            var res = await dep.EnabledAsync(guid);
            return Ok(res);
      }

      [HttpPatch("disable/{guid}")]
      public async Task<IActionResult> DisableAsync(Guid guid)
      {
            var res = await dep.DisabledAsync(guid);
            return Ok(res);
      }
}