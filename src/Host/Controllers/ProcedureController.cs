using Core.Contract.DTOs.Procedure;
using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;

namespace Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProcedureController(IProcedure proc) : ControllerBase
{
       [HttpGet("pagination")]
      public async Task<IActionResult> GetPaginationAsync([FromQuery] PaginationParams param)
      {
            var res = await proc.GetPaginationAsync(param);
            return Ok(res);
      }

      [HttpGet("{guid}")]
      public async Task<IActionResult> GetByGuidAsync(Guid guid)
      {
            var res = await proc.GetByGuidAsync(guid);
            return Ok(res);
      }

      [HttpPut]
      public async Task<IActionResult> UpdateAsync([FromBody] UpdateProcedureDto dto)
      {
            var res = await proc.UpdateAsync(dto);
            return Ok(res);
      }

      [HttpDelete("{guid}")]
      public async Task<IActionResult> DeleteAsync(Guid guid)
      {
            var res = await proc.DeleteByGuidAsync(guid);
            return Ok(res);
      }

      [HttpDelete("list")]
      public async Task<IActionResult> DeleteListAsync(IEnumerable<Guid> guids)
      {
            var res = await proc.DeleteListAsync(guids);
            return Ok(res);
      }

      [HttpPost]
      public async Task<IActionResult> CreateAsync([FromBody] CreateProcedureDto dto)
      {
            var res = await proc.CreateAsync(dto);
            return Ok(res);
      }


      [HttpPut("enable/{guid}")]
      public async Task<IActionResult> EnableAsync(Guid guid)
      {

            var res = await proc.EnabledAsync(guid);
            return Ok(res);
      }

      [HttpPut("disable/{guid}")]
      public async Task<IActionResult> DisableAsync(Guid guid)
      {


            var res = await proc.DisabledAsync(guid);
            return Ok(res);
      }
}