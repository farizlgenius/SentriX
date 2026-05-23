using Input.Contract.DTOs;
using Input.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;

namespace Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class InputController(IInput input) : ControllerBase
{
      [HttpGet("pagination")]
      public async Task<IActionResult> GetInputPagination([FromQuery] PaginationParams param)
      {
            var res = await input.GetInputPagination(param);
            return Ok(res);
      }

      [HttpPost]
      public async Task<IActionResult> CreateInputAsync([FromBody] CreateInputDto dto)
      {
            var res = await input.CreateInputAsync(dto);
            return Ok(res);
      }

      [HttpDelete("{id}")]
      public async Task<IActionResult> DeleteInputAsync(int id)
      {
            var res = await input.DeleteInputAsync(id);
            return Ok(res);
      }

      [HttpPut]
      public async Task<IActionResult> UpdateInpueAsync([FromBody]InputDto dto)
      {
            var res = await input.UpdateInputAsync(dto);
            return Ok(res);
      }

      [HttpPost("id")]
      public async Task<IActionResult> SetMaskAsync(int id,[FromBody]MaskDto dto)
      {
            var res = await input.InputMaskAsync(id,dto.IsMask);
            return Ok(res);
      }

      

      // Monitor Group

      [HttpGet("group/pagination")]
      public async Task<IActionResult> GetGroupPaginationAsync([FromQuery] PaginationParams param)
      {
            var res = await input.GetGroupPaginationAsync(param);
            return Ok(res);
      }

      [HttpPost("group")]
      public async Task<IActionResult> CreateInputGroupAsync([FromBody]CreateInputGroupDto dto)
      {
            var res = await input.CreateInputGroupAsync(dto);
            return Ok(res);
      }

      [HttpDelete("group/{id}")]
      public async Task<IActionResult> DeleteInputGroupAsync(int id)
      {
            var res = await input.DeleteInputGroupAsync(id);
            return Ok(res);
      }

      [HttpPut("group")]
      public async Task<IActionResult> UpdateInputGroupAsync([FromBody]InputGroupDto dto)
      {
            var res = await input.UpdateInputGroupAsync(dto);
            return Ok(res);
      }
}