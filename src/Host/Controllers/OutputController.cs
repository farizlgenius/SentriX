// using Microsoft.AspNetCore.Mvc;
// using SharedKernel.Domain;

// namespace Host.Controllers
// {
//       [Route("api/[controller]")]
//       [ApiController]
//       public class OutputController(IOutput output) : ControllerBase
//       {
//             [HttpGet("pagination")]
//             public async Task<IActionResult> GetPagination([FromQuery]PaginationParams param)
//             {
//                   var res = await output.GetPaginationAsync(param);
//                   return Ok(res);
//             }

//             [HttpPost]
//             public async Task<IActionResult> CreateAsync([FromBody] CreateOutputDto dto)
//             {
//                   var res = await output.CreateAsync(dto);
//                   return Ok(res);
//             }

//             [HttpGet("relay/{id}")]
//             public async Task<IActionResult> GetAvailalbleOutputByModuleIdAsync(int id)
//             {
//                   var res = await output.GetAvailalbleOutputByModuleIdAsync(id);
//                   return Ok(res);
//             }

//             [HttpGet("relay/mode")]
//             public async Task<IActionResult> GetRelayModeAsync([FromQuery]string Type)
//             {
//                   var res = await output.GetRelayModeAsync(Type);
//                   return Ok(res);
//             }

//             [HttpPost("{id}")]
//             public async Task<IActionResult> TriggerOutputAsync(int id,[FromQuery]short Command)
//             {
//                  await output.TriggerOutputAsync(id,Command);
//                   return Ok();
//             }

//             [HttpDelete("{id}")]
//             public async Task<IActionResult> DeleteOutputByIdAsync(int id)
//             {
//                   var res = await output.DeleteByIdAsync(id);
//                   return Ok(res);
//             }

//             [HttpPut]
//             public async Task<IActionResult> UpdateAsync([FromBody]OutputDto dto )
//             {
//                   var res = await output.UpdateAsync(dto);
//                   return Ok(res);
//             }

//             [HttpGet("relay/drive/mode")]
//             public async Task<IActionResult> GetRelayDriveModeAsync()
//             {
//                   var res = await output.GetRelayDriveModeAsync();
//                   return Ok(res);
//             }

//             [HttpGet("relay/offline/mode")]
//             public async Task<IActionResult> GetRelayOfflineModeAsync()
//             {
//                   var res = await output.GetRelayOfflineModeAsync();
//                   return Ok(res);
//             }

//             [HttpPost("command")]
//             public async Task<IActionResult> CommandOutputAsync([FromBody]OutputCommandDto dto)
//             {
//                   await output.CommandOutputDto(dto);
//                   return Ok();
//             }



//       }

// }

