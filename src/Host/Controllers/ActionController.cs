using Core.Contract.DTOs.Action;
using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;
using SharedKernel.Enums;

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

      [HttpGet("option/type/{type}/device/{guid}")]
      public async Task<IActionResult> GetActionOptionByTypeAndDeviceAsync(Guid guid,ActionType type)
      {
            var res = await action.GetActionOptionByTypeAndDeviceAsync(guid,type);
            return Ok(res);
      }

      [HttpGet("option/device/{guid}")]
      public async Task<IActionResult> GetActionOptionByDeviceAsync(Guid guid)
      {
            var res = await action.GetActionOptionByDeviceAsync(guid);
            return Ok(res);
      }

      [HttpPost]
      public async Task<IActionResult> CreateAsync([FromBody]CreateActionDto dto)
      {
            var res = await action.CreateAsync(dto);
            return Ok(res);
      }
}