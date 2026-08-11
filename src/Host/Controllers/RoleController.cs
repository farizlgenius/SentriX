using Core.Contract.DTOs.Role;
using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;
using SharedKernel.Exceptions;
using SharedKernel.Helpers;

namespace Host.Controllers
{
      [Route("api/[controller]")]
      [ApiController]
      public class RoleController(IRole role) : ControllerBase
      {
            [HttpGet("pagination")]
            public async Task<IActionResult> GetPaginationAsync([FromQuery] PaginationParams param)
            {
                  var res = await role.GetPaginationAsync(param);
                  return Ok(res);
            }

            [HttpGet("{guid}")]
            public async Task<IActionResult> GetAsync([FromQuery] Guid guid)
            {
                  var res = await role.GetByGuidAsync(guid);
                  return Ok(res);
            }

            [HttpPost]
            public async Task<IActionResult> CreateAsync([FromBody] CreateRoleDto dto)
            {
                  var res = await role.CreateAsync(dto);
                  return Ok(res);
            }

            [HttpPut]
            public async Task<IActionResult> UpdateAsync([FromBody] UpdateRoleDto dto)
            {
                  var res = await role.UpdateAsync(dto);
                  return Ok(res);
            }

            [HttpDelete("{guid}")]
            public async Task<IActionResult> DeleteAsync(Guid guid)
            {
                  var res = await role.DeleteByGuidAsync(guid);
                  return Ok(res);
            }

            [HttpDelete("range")]
            public async Task<IActionResult> DeleteRangeAsync([FromBody] IEnumerable<Guid> guids)
            {
                  var res = await role.DeleteRangeAsync(guids);
                  return Ok(res);
            }

            [HttpPatch("enable/{guid}")]
            public async Task<IActionResult> EnableAsync(Guid guid)
            {
                  var res = await role.EnabledAsync(guid);
                  return Ok(res);
            }

            [HttpPatch("disable/{guid}")]
            public async Task<IActionResult> DisableAsync(Guid guid)
            {
                  var res = await role.DisabledAsync(guid);
                  return Ok(res);
            }
      }
}
