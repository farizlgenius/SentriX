using Core.Contract.DTOs.Action;
using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;

namespace Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ActionController(IAction action) : ControllerBase
{
      [HttpGet("pagination")]
      public async Task<IActionResult> GetPaginationAsync([FromQuery]PaginationParams param)
      {
            var res = await action.GetPaginationAsync(param);
            return Ok(res);
      }

      [HttpPost]
      public async Task<IActionResult> CreateAsync([FromBody]CreateActionDto dto)
      {
            var res = action.CreateAsync(dto);
            return Ok(res);
      }
}