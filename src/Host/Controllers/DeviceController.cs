using Device.Contract.DTOs;
using Device.Contract.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;

namespace Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController(IDevice device) : ControllerBase
    {
        [HttpGet("option/{type}/{id}")]
        public async Task<IActionResult> GetOptionByTypeAndLocationIdAsync(int id,string type)
        {
            var res = await device.GetOptionByTypeAndLocationIdAsync(id,type);
            return Ok(res);
        }

        [HttpGet("module/option/{id}")]
        public async Task<IActionResult> GetModuleOptionByDeviceIdAsync(int id)
        {
             var res = await device.GetModuleOptionByDeviceIdAsync(id);
            return Ok(res);
        }


         [HttpGet("report")]
        public async Task<IActionResult> GetIdReportsAsync()
        {
            var res = await device.GetIdReportsAsync();
            return Ok(res);
        }

        [HttpGet("pagination")]
        public async Task<IActionResult> GetPaginationAsync([FromQuery] PaginationParams param)
        {
            var res = await device.GetPaginationAsync(param);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateDeviceDto dto)
        {
            var res = await device.CreateAsync(dto);
            return Ok(res);
        }

        [HttpGet("status/{id}")]
        public async Task<IActionResult> GetStatusByIdAsync(int id)
        {
            var res = await device.GetStatusByIdAsync(id);
            return Ok(res);
        }

        [HttpPost("reset/{id}")]
        public async Task<IActionResult> ResetDeviceAsync(int id)
        {
            var res = await device.ResetDeviceAsync(id);
            return Ok(res);
        }

        [HttpGet("module/{id}")]
        public async Task<IActionResult> GetModuleByDeviceIdAsync(int id)
        {
            var res = await device.GetModuleByDeviceIdAsync(id);
            return Ok(res);
        }

        [HttpPost("module")]
        public async Task<IActionResult> CreateModuleAsync([FromBody] CreateModuleDto dto)
        {
            var res = await device.CreateModuleAsync(dto);
            return Ok(res);
        }
        
        [HttpGet("module/status/{id}")]
        public async Task<IActionResult> GetModuleStatusByIdAsync(int id)
        {
            var res = await device.GetModuleStatusByIdAsync(id);
            return Ok(res);
        }

        [HttpPost("aero/command/{id}")]
        public async Task<IActionResult> AsciiAsync(int id,[FromBody] AeroCommandDto Command)
        {
            var res = await device.AsciiCommandAsync(id,Command);
            return Ok(res);

        }

        // Get Reader Number nad Input Number
        [HttpGet("module/reader/options/{id}")]
        public async Task<IActionResult> GetReaderOptionsByModuleIdAsync(int id)
        {
            var res = await device.GetReaderOptionsByModuleIdAsync(id);
            return Ok(res);
        }

        [HttpGet("module/input/options/{id}")]
        public async Task<IActionResult> GetInputOptionsByModuleIdAsync(int id)
        {
            var res = await device.GetInputOptionsByModuleIdAsync(id);
            return Ok(res);
        }

        [HttpGet("module/relay/options/{id}")]
        public async Task<IActionResult> GetRelayOptionsByModuleIdAsync(int id)
        {
            var res = await device.GetRelayOptionsByModuleIdAsync(id);
            return Ok(res);
        }



    }
}
