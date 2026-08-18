using Core.Contract.DTOs.License;
using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers;


[Route("api/[controller]")]
[ApiController]
public class LicenseController(ILicense lic) : ControllerBase
{

      [HttpPost("demo")]
      public async Task<IActionResult> RequestDemoAsync([FromBody] DemoLicenseDto dto)
      {
            var res = await lic.RequestDemoAsync(dto);
            return Ok(res);
      }

      [HttpPost]
      public async Task<IActionResult> DownloadAsync([FromBody] DownloadLicenseDto dto)
      {
            var res = await lic.DownloadAsync(dto);
            return Ok(res);
      }

      [HttpPost("activate")]
      public async Task<IActionResult> ActivateAsync([FromBody] ActivateDto dto)
      {
            var res = await lic.ActivateAsync(dto);
            return Ok(res);
      }
}