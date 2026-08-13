using Core.Contract.DTOs.License;
using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers;


[Route("api/[controller]")]
[ApiController]
public class LicenseController(ILicense lic) : ControllerBase
{
      [HttpGet]
      public async Task<IActionResult> CheckLicenseAsync()
      {
            var res = await lic.CheckLicenseAsync();
            return Ok(res);
      }

      [HttpGet("identity")]
      public async Task<IActionResult> GetMachineId()
      {
            var res = await lic.GetMachineIdAsync();
            return Ok(res);
      }

      [HttpPost("generate/demo")]
      public async Task<IActionResult> GenerateDemoLicenseAsync([FromBody] CreateDemoLicenseDto dto)
      {
            // TO DO : Implement Demo License Generation
            var res = await lic.GenerateDemoAsync(dto);
            return Ok(res);
      }



      [HttpPost]
      public async Task<IActionResult> CreateAsync()
      {
            var res = await lic.GenerateLicenseAsync();
            return Ok(res);
      }

}