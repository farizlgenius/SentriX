using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Setting.Contract.DTOs;
using Setting.Contract.DTOs.PasswordRule;
using Setting.Contract.Interfaces;
using SharedKernel.Domain;

namespace Host.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class SettingController(
        IPasswordRule pass,
        ISetting setting
  ) : ControllerBase
  {
    [HttpGet("password/rule")]
    public async Task<IActionResult> GetPassowrdRuleAsync()
    {
      var res = await pass.GetAsync();
      return Ok(res);
    }

    [HttpPut("password/rule")]
    public async Task<IActionResult> CreatePasswordRuleAsync([FromBody] UpdatePasswordRuleDto dto)
    {
      var res = await pass.UpdateAsync(dto);
      return Ok(res);
    }

    [HttpGet("aero")]
    public async Task<IActionResult> GetAeroDriverSettingAsync()
    {
      var res = await setting.GetAeroDriverSettingAsync();
      return Ok(res);
    }


  }
}