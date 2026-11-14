


using Core.Contract.DTOs.Trigger;
using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;

namespace Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TriggerController(ITrigger trigger) : ControllerBase
{
      [HttpGet("pagination")]
      public async Task<IActionResult> GetPaginationAsync([FromQuery] PaginationParams param)
      {
            var res = await trigger.GetPaginationAsync(param);
            return Ok(res);
      }

      [HttpGet("{guid}")]
      public async Task<IActionResult> GetByGuidAsync(Guid guid)
      {
            var res = await trigger.GetByGuidAsync(guid);
            return Ok(res);
      }

      [HttpPut]
      public async Task<IActionResult> UpdateAsync([FromBody] UpdateTriggerDto dto)
      {
            var res = await trigger.UpdateAsync(dto);
            return Ok(res);
      }

      [HttpDelete("{guid}")]
      public async Task<IActionResult> DeleteAsync(Guid guid)
      {
            var res = await trigger.DeleteByGuidAsync(guid);
            return Ok(res);
      }

      [HttpDelete("list")]
      public async Task<IActionResult> DeleteListAsync(IEnumerable<Guid> guids)
      {
            var res = await trigger.DeleteListAsync(guids);
            return Ok(res);
      }

      [HttpPost]
      public async Task<IActionResult> CreateAsync([FromBody] CreateTriggerDto dto)
      {
            var res = await trigger.CreateAsync(dto);
            return Ok(res);
      }

      

      [HttpGet("source")]
      public async Task<IActionResult> GetSourceAsync()
      {
            var res = await trigger.GetSourceAsync();
            return Ok(res);
      }


      [HttpPut("enable/{guid}")]
      public async Task<IActionResult> EnableAsync(Guid guid)
      {

            var res = await trigger.EnabledAsync(guid);
            return Ok(res);
      }

      [HttpPut("disable/{guid}")]
      public async Task<IActionResult> DisableAsync(Guid guid)
      {


            var res = await trigger.DisabledAsync(guid);
            return Ok(res);
      }

      [HttpGet("type/{source}")]
      public async Task<IActionResult> GetTypeAsync(Guid source)
      {
            var res = await trigger.GetTypeAsync(source);
            return Ok(res);
      }

      [HttpGet("code/{type}")]
      public async Task<IActionResult> GetCodeAsync(Guid type)
      {
            var res = await trigger.GetCodeAsync(type);
            return Ok(res);
      }

      // Variable 
      [HttpGet("variable/pagination")]
      public async Task<IActionResult> GetVariablePaginationAsync([FromQuery] PaginationParams param)
      {
            var res = await trigger.GetPaginationAsync(param);
            return Ok(res);
      }

      [HttpGet("variable/{guid}")]
      public async Task<IActionResult> GetVariableByGuidAsync(Guid guid)
      {
            var res = await trigger.GetByGuidAsync(guid);
            return Ok(res);
      }

      [HttpPut("variable")]
      public async Task<IActionResult> UpdateVariableAsync([FromBody] UpdateTriggerDto dto)
      {
            var res = await trigger.UpdateAsync(dto);
            return Ok(res);
      }

      [HttpDelete("variable/{guid}")]
      public async Task<IActionResult> DeleteVariableAsync(Guid guid)
      {
            var res = await trigger.DeleteByGuidAsync(guid);
            return Ok(res);
      }

      [HttpDelete("variable/list")]
      public async Task<IActionResult> DeleteVariableListAsync(IEnumerable<Guid> guids)
      {
            var res = await trigger.DeleteListAsync(guids);
            return Ok(res);
      }

      [HttpPost("variable")]
      public async Task<IActionResult> CreateVariableAsync([FromBody] CreateTriggerDto dto)
      {
            var res = await trigger.CreateAsync(dto);
            return Ok(res);
      }


      [HttpPut("variable/enable/{guid}")]
      public async Task<IActionResult> EnableVariableAsync(Guid guid)
      {

            var res = await trigger.EnabledAsync(guid);
            return Ok(res);
      }

      [HttpPut("variable/disable/{guid}")]
      public async Task<IActionResult> DisableVariableAsync(Guid guid)
      {


            var res = await trigger.DisabledAsync(guid);
            return Ok(res);
      }


}