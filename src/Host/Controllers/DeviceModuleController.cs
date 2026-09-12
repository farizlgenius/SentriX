using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Enums;

namespace Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeviceModuleController(IDeviceModule module) : ControllerBase
{
  [HttpGet("location/{locationGuid}/vendor/{vendor}")]
  public async Task<IActionResult> GetByVendorAndLocationAsync(Guid locationGuid, Vendor vendor)
  {
    var res = await module.GetByVendorAndLocationAsync(locationGuid, vendor);
    return Ok(res);
  }
}