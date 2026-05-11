using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Role.Contract.DTOs;
using Role.Contract.Interfaces;

namespace Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController(IRole role) : ControllerBase
    {
        [HttpGet("location/{id}")]
        public async Task<IActionResult> GetByLocationIdAsync(int id)
        {
            var res = await role.GetByLocationIdAsync(id);
            return Ok(res);
        }

        [HttpGet("pagination")]
        public async Task<IActionResult> GetPaginationWithLocationIdAsync([FromQuery] int LocationId, [FromQuery] int Page, [FromQuery] int PageSize)
        {
            var res = await role.GetPaginationWithLocationIdAsync(LocationId, Page, PageSize);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateRoleDto dto)
        {
            var res = await role.CreateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteByIdAsync(int id)
        {
            var res = await role.DeleteByIdAsync(id);
            return Ok(res);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateRoleDto dto)
        {
            var res = await role.UpdateAsync(dto);
            return Ok(res);
        }

        [HttpGet("feature")]
        public async Task<IActionResult> GetFeaturesAsync()
        {
            var res = await role.GetFeaturesAsync();
            return Ok(res);
        }

        [HttpPost("delete/range")]
        public async Task<IActionResult> DeleteRangeAsync([FromBody] RangeIdDto Ids)
        {
            var res = await role.DeleteRangeAsync(Ids);
            return Ok(res);
        }
    }
}
