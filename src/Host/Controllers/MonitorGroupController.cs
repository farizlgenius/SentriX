// using Core.Contract.Interfaces;
// using Microsoft.AspNetCore.Mvc;

// namespace Host.Controllers;

// [Route("api/[controller]")]
// [ApiController]
// public class MonitorGroupController(IMonitorGroup group) : ControllerBase
// {
//       // Monitor Group

//       [HttpGet("group/pagination")]
//       public async Task<IActionResult> GetGroupPaginationAsync([FromQuery] PaginationParams param)
//       {
//             var res = await input.GetGroupPaginationAsync(param);
//             return Ok(res);
//       }

//       [HttpPost("group")]
//       public async Task<IActionResult> CreateInputGroupAsync([FromBody]CreateInputGroupDto dto)
//       {
//             var res = await input.CreateInputGroupAsync(dto);
//             return Ok(res);
//       }

//       [HttpDelete("group/{id}")]
//       public async Task<IActionResult> DeleteInputGroupAsync(int id)
//       {
//             var res = await input.DeleteInputGroupAsync(id);
//             return Ok(res);
//       }

//       [HttpPut("group")]
//       public async Task<IActionResult> UpdateInputGroupAsync([FromBody]InputGroupDto dto)
//       {
//             var res = await input.UpdateInputGroupAsync(dto);
//             return Ok(res);
//       }

//       [HttpGet("mode")]
//       public async Task<IActionResult> GetInputModeAsync()
//       {
//             var res = await input.GetInputModeAsync();
//             return Ok(res);
//       }
      
// }