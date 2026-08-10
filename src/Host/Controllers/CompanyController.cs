using Core.Contract.DTOs.Company;
using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;

namespace Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompanyController(ICompany com) : ControllerBase
{
  [HttpGet("pagination")]
  public async Task<IActionResult> GetPaginationAsync([FromQuery] PaginationParams param)
  {
    var res = await com.GetPaginationAsync(param);
    return Ok(res);
  }

  [HttpGet("{guid}")]
  public async Task<IActionResult> GetAsync([FromQuery] Guid guid)
  {
    var res = await com.GetByGuidAsync(guid);
    return Ok(res);
  }

  [HttpPost]
  public async Task<IActionResult> CreateAsync([FromBody] CreateCompanyDto dto)
  {
    var res = await com.CreateAsync(dto);
    return Ok(res);
  }

  [HttpPut]
  public async Task<IActionResult> UpdateAsync([FromBody] UpdateCompanyDto dto)
  {
    var res = await com.UpdateAsync(dto);
    return Ok(res);
  }

  [HttpDelete("{guid}")]
  public async Task<IActionResult> DeleteAsync(Guid guid)
  {
    var res = await com.DeleteByGuidAsync(guid);
    return Ok(res);
  }

  [HttpDelete("range")]
  public async Task<IActionResult> DeleteRangeAsync([FromBody] IEnumerable<Guid> guids)
  {
    var res = await com.DeleteRangeAsync(guids);
    return Ok(res);
  }
}