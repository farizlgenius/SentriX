using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Operator.Contract.DTOs;
using Operator.Contract.Interfaces;
using SharedKernel.Domain;
using SharedKernel.Exceptions;
using SharedKernel.Helpers;

namespace Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperatorController(IOperator oper) : ControllerBase
    {
        [HttpGet("pagination")]
        public async Task<IActionResult> GetPagination([FromQuery]PaginationParams param)
        {
            var tenants = User.FindFirst("tenants")?.Value ?? "";

            if(!ValidationHelper.ValidateTenants(tenants,param.locationId))
                throw new ForbiddenException(MessageHelper.Location.LocationNotAllow);
            
            var res = await oper.GetPagination(param);
                return Ok(res);

        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateOperatorDto dto)
        {
            var res = await oper.CreateAsync(dto);
            return Ok(res);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateOperatorDto dto)
        {
            var res = await oper.UpdateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteByIdAsync(int id)
        {
            var res = await oper.DeleteByIdAsync(id);
            return Ok(res);
        }

        [HttpGet("password/rule")]
        public async Task<IActionResult> GetPassowrdRuleAsync()
        {
            var res = await oper.GetPassowrdRuleAsync();
            return Ok(res);
        }

        [HttpPost("password/rule")]
        public async Task<IActionResult> CreatePasswordRuleAsync([FromBody] PasswordRuleDto dto)
        {
            var res = await oper.CreatePasswordRuleAsync(dto);
            return Ok(res);
        }
    }
}
