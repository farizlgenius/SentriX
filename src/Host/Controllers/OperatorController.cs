using Core.Contract.DTOs.Operator;
using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Setting.Contract.Interfaces;
using SharedKernel.Domain;
using SharedKernel.Exceptions;
using SharedKernel.Helpers;

namespace Host.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class OperatorController(
    IOperator oper,
    IPasswordRule pass
    ) : ControllerBase
  {
    [HttpGet("pagination")]
    public async Task<IActionResult> GetPagination([FromQuery] PaginationParams param)
    {
      // var tenants = User.FindFirst("tenants")?.Value ?? "";

      // if (!ValidationHelper.ValidateTenants(tenants, param.locationId))
      //   throw new ForbiddenException(MessageHelper.Location.LocationNotAllow);

      var res = await oper.GetPaginationAsync(param);
      return Ok(res);

    }


    [HttpGet("{guid}")]
    public async Task<IActionResult> GetAsync([FromQuery] Guid guid)
    {
      var res = await oper.GetByGuidAsync(guid);
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

    [HttpDelete("{guid}")]
    public async Task<IActionResult> DeleteAsync(Guid guid)
    {
      var res = await oper.DeleteByGuidAsync(guid);
      return Ok(res);
    }

    [HttpDelete("range")]
    public async Task<IActionResult> DeleteRangeAsync([FromBody] IEnumerable<Guid> guids)
    {
      var res = await oper.DeleteRangeAsync(guids);
      return Ok(res);
    }

    [HttpPatch]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordDto dto)
    {
      var res = await oper.ChangePasswordAsync(dto);
      return Ok(res);
    }

    [HttpPatch("enable/{guid}")]
    public async Task<IActionResult> EnableAsync(Guid guid)
    {
      var res = await oper.EnabledAsync(guid);
      return Ok(res);
    }

    [HttpPatch("disable/{guid}")]
    public async Task<IActionResult> DisableAsync(Guid guid)
    {
      var res = await oper.DisabledAsync(guid);
      return Ok(res);
    }


  }
}
