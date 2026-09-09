using Core.Contract.DTOs.Door;
using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;

namespace Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DoorController(IDoor door) : ControllerBase
{
  [HttpGet("pagination")]
  public async Task<IActionResult> GetDoorPaginationAsync([FromQuery] PaginationParams param)
  {
    var res = await door.GetPaginationAsync(param);
    return Ok(res);
  }

  [HttpGet("{guid}")]
  public async Task<IActionResult> GetByGuidAsync([FromQuery] Guid guid)
  {
    var res = await door.GetByGuidAsync(guid);
    return Ok(res);
  }

  [HttpGet("location/{guid}")]
  public async Task<IActionResult> GetByLocationAsync([FromQuery] Guid guid)
  {
    var res = await door.GetByLocationAsync(guid);
    return Ok(res);
  }

  [HttpPost]
  public async Task<IActionResult> CreateAsync([FromBody] CreateDoorDto dto)
  {
    var res = await door.CreateAsync(dto);
    return Ok(res);
  }

  [HttpPut]
  public async Task<IActionResult> UpdateAsync([FromBody] UpdateDoorDto dto)
  {
    var res = await door.UpdateAsync(dto);
    return Ok(res);
  }

  [HttpDelete("{guid}")]
  public async Task<IActionResult> DeleteAsync(Guid guid)
  {
    var res = await door.DeleteByGuidAsync(guid);
    return Ok(res);
  }

  [HttpDelete("list")]
  public async Task<IActionResult> DeleteListAsync([FromBody] IEnumerable<Guid> guids)
  {
    var res = await door.DeleteListAsync(guids);
    return Ok(res);
  }

  [HttpPatch("enable/{guid}")]
  public async Task<IActionResult> EnabledAsync([FromQuery] Guid guid)
  {
    var res = await door.EnabledAsync(guid);
    return Ok(res);
  }

  [HttpPatch("diable/{guid}")]
  public async Task<IActionResult> DisabledAsync([FromQuery] Guid guid)
  {
    var res = await door.EnabledAsync(guid);
    return Ok(res);
  }

  // [HttpGet("reader/mode")]
  // public async Task<IActionResult> GetReaderModeAsync()
  // {
  //   var res = await door.GetReaderModeAsync();
  //   return Ok(res);
  // }

  // [HttpGet("strike/mode")]
  // public async Task<IActionResult> GetStrikeModeAsync()
  // {
  //   var res = await door.GetStrikeModeAsync();
  //   return Ok(res);
  // }

  // [HttpGet("apb/mode")]
  // public async Task<IActionResult> GetApbModeAsync()
  // {
  //   var res = await door.GetApbModeAsync();
  //   return Ok(res);
  // }

  // [HttpGet("mode")]
  // public async Task<IActionResult> GetDoorModeAsync()
  // {
  //   var res = await door.GetDoorModeAsync();
  //   return Ok(res);
  // }

  // [HttpGet("acsflag")]
  // public async Task<IActionResult> GetAccessControlFlagAsync()
  // {
  //   var res = await door.GetAccessControlFlagAsync();
  //   return Ok(res);
  // }

  // [HttpGet("spareflag")]
  // public async Task<IActionResult> GetSpareFlagAsync()
  // {
  //   var res = await door.GetSpareFlagAsync();
  //   return Ok(res);
  // }

  // [HttpGet("osdp/baudrate")]
  // public async Task<IActionResult> GetOsdpBaudrateAsync()
  // {
  //   var res = await door.GetOsdpBaudrateAsync();
  //   return Ok(res);
  // }

  // [HttpGet("option/{LocationId}")]
  // public async Task<IActionResult> GetDoorOptionByLocationIdAsync(int LocationId)
  // {
  //   var res = await door.GetDoorOptionByLocationIdAsync(LocationId);
  //   return Ok(res);
  // }

}