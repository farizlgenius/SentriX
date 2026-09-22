using Core.Contract.DTOs.Device;
using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;
using SharedKernel.Enums;

namespace Host.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class DeviceController(IDevice device) : ControllerBase
  {
    [HttpGet("pagination")]
    public async Task<IActionResult> GetPaginationAsync([FromQuery] PaginationParams param)
    {
      var res = await device.GetPaginationAsync(param);
      return Ok(res);
    }

    [HttpGet("scan")]
    public async Task<IActionResult> GetScanDeviceAsync()
    {
      var res = await device.GetScanDeviceAsync();
      return Ok(res);
    }

    [HttpGet("{guid}")]
    public async Task<IActionResult> GetByGuidAsync(Guid guid)
    {
      var res = await device.GetByGuidAsync(guid);
      return Ok(res);
    }

    [HttpGet("status/{guid}")]
    public async Task<IActionResult> GetStatusAsync(Guid guid)
    {
      var res = await device.GetStatusAsync(guid);
      return Ok(res);
    }

    [HttpPost("status")]
    public async Task<IActionResult> GetStatusesAsync([FromBody] IEnumerable<Guid> guids)
    {
      var res = await device.GetStatusesAsync(guids);
      return Ok(res);
    }


    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateDeviceDto dto)
    {
      var res = await device.CreateAsync(dto);
      return Ok(res);
    }

    [HttpDelete("{guid}")]
    public async Task<IActionResult> DeleteByGuidAsync(Guid guid)
    {
      var res = await device.DeleteByGuidAsync(guid);
      return Ok(res);
    }


    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateDeviceDto dto)
    {
      var res = await device.UpdateAsync(dto);
      return Ok(res);
    }

    [HttpDelete("range")]
    public async Task<IActionResult> DeleteRangeAsync([FromBody] IEnumerable<Guid> guids)
    {
      var res = await device.DeleteListAsync(guids);
      return Ok(res);
    }

    [HttpGet("config/{guid}")]
    public async Task<IActionResult> ConfigurationAsync(Guid guid)
    {
      var res = await device.GetConfigurationAsync(guid);
      return Ok(res);
    }

    [HttpPost("reset/{guid}")]
    public async Task<IActionResult> ResetAsync(Guid guid)
    {
      var res = await device.ResetAsync(guid);
      return Ok(res);
    }

    

  }
}
