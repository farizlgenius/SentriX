using Microsoft.AspNetCore.Mvc;
using Setting.Contract.DTOs;
using Setting.Contract.Interfaces;
using SharedKernel.Domain;

namespace Host.Controllers
{
      [Route("api/[controller]")]
      [ApiController]
      public class SettingController(
            ICardFormat cfmt
      ) : ControllerBase
      {
            [HttpPost("cfmt")]
            public async Task<IActionResult> CreateCardFormatAsync([FromBody]CreateCardFormatDto dto)
            {
                  var res = await cfmt.CreateAsync(dto);
                  return Ok(res);
            }

            [HttpGet("cfmt/pagination")]
            public async Task<IActionResult> GetCardFormatPaginationAsync([FromQuery]PaginationParams param)
            {
                  var res = await cfmt.GetCardFormatPaginationAsync(param);
                  return Ok(res);
            }

            [HttpGet("cfmt/{id}")]
            public async Task<IActionResult> GetCardFormatByIdAsync(int id)
            {
                  var res = await cfmt.GetByIdAsync(id);
                  return Ok(res);
            }

            [HttpDelete("cfmt/{id}")]
            public async Task<IActionResult> DeleteCardFormatByIdAsync(int id)
            {
                  var res = await cfmt.DeleteByIdAsync(id);
                  return Ok(res);
            }

            [HttpPut("cfmt")]
            public async Task<IActionResult> UpdateCardFormatAsync([FromBody]CardFormatDto dto)
            {
                  var res = await cfmt.UpdateAsync(dto);
                  return Ok(res);
            }

      }
}