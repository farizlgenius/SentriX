using Group.Contract.DTOs;
using Group.Contract.Interfaces;
using Input.Contract.DTOs;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;

namespace Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GroupController(IGroup group) : ControllerBase
{
      [HttpGet("pagination")]
      public async Task<IActionResult> GetPaginationAsync([FromQuery]PaginationParams param)
      {
            var res = await group.GetPaginationAsync(param);
            return Ok(res);
      }

      [HttpGet("{location}")]
      public async Task<IActionResult> GetByLocationIdAsync(int location)
      {
            var res = await group.GetByLocationIdAsync(location);
            return Ok(res);
      }

      [HttpPost]
      public async Task<IActionResult> CreateAsync([FromBody] CreateGroupDto dto)
      {
            var res = await group.CreateAsync(dto);
            return Ok(res);
      }

      [HttpDelete("{guid}")]
      public async Task<IActionResult> DeleteByGuidAsync(Guid guid)
      {
            var res = await group.DeleteByGuidAsync(guid);
            return Ok(res);
      }

      [HttpPut]
      public async Task<IActionResult> UpdateAsync([FromBody] GroupDto dto)
      {
            var res = await group.UpdateAsync(dto);
            return Ok(res);
      }
}