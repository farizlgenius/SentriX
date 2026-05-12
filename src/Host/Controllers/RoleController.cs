using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Role.Contract.DTOs;
using Role.Contract.Interfaces;
using SharedKernel.Domain;
using SharedKernel.Exceptions;
using SharedKernel.Helpers;

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
        public async Task<IActionResult> GetPagination([FromQuery] PaginationParams param)
        {
            var tenants = User.FindFirst("tenants")?.Value ?? "";

            if(!ValidationHelper.ValidateTenants(tenants,param.locationId))
                throw new ForbiddenException(MessageHelper.Location.LocationNotAllow);
            
             var res = await role.GetPagination(param);
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
