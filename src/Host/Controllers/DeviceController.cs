// using Microsoft.AspNetCore.Mvc;
// using SharedKernel.Domain;

// namespace Host.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class DeviceController(IDevice device) : ControllerBase
//     {
//         [HttpGet("option/{type}/{id}")]
//         public async Task<IActionResult> GetOptionByTypeAndLocationIdAsync(int id,string type)
//         {
//             var res = await device.GetOptionByTypeAndLocationIdAsync(id,type);
//             return Ok(res);
//         }

//         [HttpGet("module/option/{guid}")]
//         public async Task<IActionResult> GetModuleOptionByDeviceIdAsync(Guid guid)
//         {
//              var res = await device.GetModuleOptionByDeviceGuidAsync(guid);
//             return Ok(res);
//         }


//          [HttpGet("report")]
//         public async Task<IActionResult> GetIdReportsAsync()
//         {
//             var res = await device.GetIdReportsAsync();
//             return Ok(res);
//         }

//         [HttpGet("pagination")]
//         public async Task<IActionResult> GetPaginationAsync([FromQuery] PaginationParams param)
//         {
//             var res = await device.GetPaginationAsync(param);
//             return Ok(res);
//         }

//         [HttpPost]
//         public async Task<IActionResult> CreateAsync([FromBody] CreateDeviceDto dto)
//         {
//             var res = await device.CreateAsync(dto);
//             return Ok(res);
//         }

//         [HttpGet("status/{guid}")]
//         public async Task<IActionResult> GetStatusByGuidAsync(Guid guid)
//         {
//             var res = await device.GetStatusByGuidAsync(guid);
//             return Ok(res);
//         }

//         [HttpPost("reset/{guid}")]
//         public async Task<IActionResult> ResetDeviceAsync(Guid guid)
//         {
//             await device.ResetDeviceAsync(guid);
//             return Ok();
//         }

//         [HttpGet("module/{guid}")]
//         public async Task<IActionResult> GetModuleByDeviceGuidAsync(Guid guid)
//         {
//             var res = await device.GetModuleByDeviceGuidAsync(guid);
//             return Ok(res);
//         }

//         [HttpPost("module")]
//         public async Task<IActionResult> CreateModuleAsync([FromBody] CreateModuleDto dto)
//         {
//             var res = await device.CreateModuleAsync(dto);
//             return Ok(res);
//         }
        
//         [HttpGet("module/status/{guid}")]
//         public async Task<IActionResult> GetModuleStatusByIdAsync(Guid guid)
//         {
//             await device.GetModuleStatusByGuidAsync(guid);
//             return Ok();
//         }

//         [HttpPost("aero/command/{guid}")]
//         public async Task<IActionResult> AsciiAsync(Guid guid,[FromBody] AeroCommandDto Command)
//         {
//            await device.AsciiCommandAsync(guid,Command);
//             return Ok();

//         }

//         // Get Reader Number nad Input Number
//         [HttpGet("module/reader/options/{guid}")]
//         public async Task<IActionResult> GetReaderOptionsByModuleIdAsync(Guid guid)
//         {
//             var res = await device.GetReaderOptionsByModuleGuidAsync(guid);
//             return Ok(res);
//         }

//         [HttpGet("module/input/options/{guid}")]
//         public async Task<IActionResult> GetInputOptionsByModuleIdAsync(Guid guid)
//         {
//             var res = await device.GetInputOptionsByModuleIdAsync(guid);
//             return Ok(res);
//         }

//         [HttpGet("module/relay/options/{guid}")]
//         public async Task<IActionResult> GetRelayOptionsByModuleIdAsync(Guid guid)
//         {
//             var res = await device.GetRelayOptionsByModuleIdAsync(guid);
//             return Ok(res);
//         }

//         [HttpGet("event/{guid}")]
//         public async Task<IActionResult> GetEventStatusAsync(Guid guid)
//         {
//            await device.GetEventStatusAsync(guid);
//             return Ok();
//         }

//         [HttpPost("event")]
//         public async Task<IActionResult> GetEventStatusAsync([FromBody] SetEventDto dto)
//         {
//             await device.SetEventStatusAsync(dto);
//             return Ok();
//         }

//         [HttpPost("upload/{guid}")]
//         public async Task<IActionResult> UploadDeviceAsync(Guid guid)
//         {
//            await device.UploadDeviceAsync(guid);
//             return Ok();
//         }

//         [HttpDelete("{guid}")]
//         public async Task<IActionResult> DeleteDeviceAsync(Guid guid)
//         {
//             var res = await device.DeleteDeviceAsync(guid);
//             return Ok();
//         }


//         // Amico
//         [HttpPost("amico/connect")]
//         public async Task<IActionResult> GetAmicoDeviceInformationAsync([FromBody]AmicoStartSessionDto dto)
//         {
//             var res = await device.GetAmicoDeviceInformationAsync(dto);
//             return Ok(res);
//         }

        


//     }
// }
