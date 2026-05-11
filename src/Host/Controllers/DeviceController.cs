using Device.Contract.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController(IDevice device) : ControllerBase
    {
         [HttpGet("report")]
        public async Task<IActionResult> GetIdReportsAsync()
        {
            var res = await device.GetIdReportsAsync();
            return Ok(res);
        }


    }
}
