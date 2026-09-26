using System.Security.Claims;
using Core.Contract.DTOs.Output;
using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;

namespace Host.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class OutputController(IOutput output) : ControllerBase
  {
    [HttpGet("pagination")]
    public async Task<IActionResult> GetPagination([FromQuery] PaginationParams param)
    {
      var res = await output.GetPaginationAsync(param);
      return Ok(res);
    }

    [HttpGet("{guid}")]
    public async Task<IActionResult> GetByGuidAsync(Guid guid)
    {
      var res = await output.GetByGuidAsync(guid);
      return Ok(res);
    }

    [HttpGet("location/{guid}")]
    public async Task<IActionResult> GetByLocationAsync(Guid guid)
    {
      var res = await output.GetByLocationAsync(guid);
      return Ok(res);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateOutputDto dto)
    {
      

      var res = await output.CreateAsync(dto);
      return Ok(res);
    }


    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateOutputDto dto)
    {
      

      var res = await output.UpdateAsync(dto);
      return Ok(res);
    }

    [HttpDelete("{guid}")]
    public async Task<IActionResult> DeleteByGuidAsync(Guid guid)
    {
      

      var res = await output.DeleteByGuidAsync(guid);
      return Ok(res);
    }

    [HttpDelete("list")]
    public async Task<IActionResult> DeleteRangeAsync([FromBody] IEnumerable<Guid> guids)
    {
      

      var res = await output.DeleteListAsync(guids);
      return Ok(res);
    }

    [HttpPatch("enable/{guid}")]
    public async Task<IActionResult> EnabledByGuidAsync(Guid guid)
    {
      

      var res = await output.EnabledAsync(guid);
      return Ok(res);
    }

    [HttpPatch("disable/{guid}")]
    public async Task<IActionResult> DisabledByGuidAsync(Guid guid)
    {
      

      var res = await output.DisabledAsync(guid);
      return Ok(res);
    }




  }

}

