using Core.Contract.Interfaces;
using Host.Helpers;
using Microsoft.AspNetCore.Mvc;
using Setting.Contract.DTOs;
using Setting.Contract.DTOs.PasswordRule;
using Setting.Contract.Interfaces;
using SharedKernel.Domain;

namespace Host.Controllers
{
      [Route("api/[controller]")]
      [ApiController]
      public class UtilityController(
            AeroCommandDecoder decoder
      ) : ControllerBase
      {
            [HttpPost("decode")]
            public async Task<IActionResult> DecodeAsync([FromBody] string com)
            {
                  var res = decoder.DecodeCommand(com);
                  return Ok(res);
            }


      }
}