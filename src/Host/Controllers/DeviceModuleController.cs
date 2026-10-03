using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Enums;

namespace Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeviceModuleController(IDeviceModule module) : ControllerBase
{
  [HttpGet("device/{guid}")]
  public async Task<IActionResult> GetByDeviceAsync(Guid guid)
  {
    var res = await module.GetByDeviceAsync(guid);
    return Ok(res);
  }

  [HttpGet("status/{guid}")]
  public async Task<IActionResult> GetSatusAsync(Guid guid)
  {
    var res = await module.GetStatusAsync(guid);
    return Ok(res);
  }

  [HttpGet("option/{guid}")]
  public async Task<IActionResult> GetOptionByDeviceAsync(Guid guid)
  {
    var res = await module.GetOptionByDeviceAsync(guid);
    return Ok(res);
  }

  [HttpGet("location/{locationGuid}/vendor/{vendor}")]
  public async Task<IActionResult> GetByVendorAndLocationAsync(Guid locationGuid, Vendor vendor)
  {
    var res = await module.GetByVendorAndLocationAsync(locationGuid, vendor);
    return Ok(res);
  }

  [HttpGet("reader/{guid}")]
  public async Task<IActionResult> GetReaderSlotAsync(Guid guid)
  {
    var res = await module.GetReaderSlotAsync(guid);
    return Ok(res);
  }

  [HttpGet("input/{guid}")]
  public async Task<IActionResult> GetInputSlotAsync(Guid guid)
  {
    var res = await module.GetInputSlotAsync(guid);
    return Ok(res);
  }

  [HttpGet("output/{guid}")]
  public async Task<IActionResult> GetOutputSlotAsync(Guid guid)
  {
    var res = await module.GetOutputSlotAsync(guid);
    return Ok(res);
  }
}