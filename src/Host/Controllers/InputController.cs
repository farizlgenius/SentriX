using Core.Contract.DTOs.Input;
using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;

namespace Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class InputController(IInput input) : ControllerBase
{
      [HttpGet("pagination")]
      public async Task<IActionResult> GetPaginationAsync([FromQuery] PaginationParams param)
      {
            var res = await input.GetPaginationAsync(param);
            return Ok(res);
      }

      [HttpPost]
      public async Task<IActionResult> Create([FromBody] CreateInputDto dto)
      {
            var res = await input.CreateAsync(dto);
            return Ok(res);
      }

      [HttpDelete("{guid}")]
      public async Task<IActionResult> DeleteAsync(Guid guid)
      {
            var res = await input.DeleteByGuidAsync(guid);
            return Ok(res);
      }

      [HttpPut]
      public async Task<IActionResult> UpdateAsync([FromBody]UpdateInputDto dto)
      {
            var res = await input.UpdateAsync(dto);
            return Ok(res);
      }

      // [HttpPost("mask/{id}")]
      // public async Task<IActionResult> SetMaskAsync(int id,[FromBody]MaskDto dto)
      // {
      //       await input.InputMaskAsync(id,dto.IsMask);
      //       return Ok();
      // }

      // [HttpGet("input/{id}")]
      // public async Task<IActionResult> GetAvailalbleInputByModuleIdAsync(int id)
      // {
      //             var res = await input.GetAvailalbleInputByModuleIdAsync(id);
      //             return Ok(res);
      // }

      

      
}