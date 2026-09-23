
using Core.Contract.DTOs.Utility;
using Core.Contract.Interfaces;
using Host.Helpers;
using Microsoft.AspNetCore.Mvc;
using Setting.Contract.DTOs;
using Setting.Contract.DTOs.PasswordRule;
using Setting.Contract.Interfaces;
using SharedKernel.Domain;
using SharedKernel.Enums;

namespace Host.Controllers
{
      [Route("api/[controller]")]
      [ApiController]
      public class UtilityController(
           IUtility utility
      ) : ControllerBase
      {
            // [HttpPost("aero/decode")]
            // public async Task<IActionResult> DecodeAsync([FromBody] string com)
            // {
            //       var res = utility.DecodeCommandAsync(com,Vendor.aero);
            //       return Ok(res);
            // }

            [HttpPost("aero/decode")]
            public async Task<IActionResult> DecodeWithColorAsync([FromBody] DecodeCommandRequest com)
            {
                  var res = utility.DecodeCommandWithColorAsync(com.Command,Vendor.aero);
                  return Ok(res);
            }


      }
}