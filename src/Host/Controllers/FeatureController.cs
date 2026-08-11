using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FeatureController(IFeature feature) : ControllerBase
{
      [HttpGet]
      public async Task<IActionResult> GetAsync()
      {
            var res = await feature.GetAsync();
            return Ok(res);
      }
}