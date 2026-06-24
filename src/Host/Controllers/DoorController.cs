using Door.Contract.DTOs;
using Door.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;

namespace Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DoorController(IDoor door) : ControllerBase
{
      [HttpGet("pagination")]
      public async Task<IActionResult> GetDoorPaginationAsync([FromQuery]PaginationParams param)
      {
            var res = await door.GetDoorPaginationAsync(param);
            return Ok(res);
      }

      [HttpPost]
      public async Task<IActionResult> CreateAsync([FromBody]CreateDoorDto dto)
      {
            var res = await door.CreateAsync(dto);
            return Ok(res);
      }

      [HttpPut]
      public async Task<IActionResult> UpdateAsync([FromBody]DoorDto dto)
      {
            var res = await door.UpdateAsync(dto);
            return Ok(res);
      }

      [HttpDelete("{id}")]
      public async Task<IActionResult> DeleteAsync(int id)
      {
            var res = await door.DeleteAsync(id);
            return Ok(res);
      }

      [HttpGet("reader/mode")]
      public async Task<IActionResult> GetReaderModeAsync()
      {
            var res = await door.GetReaderModeAsync();
            return Ok(res);
      }

      [HttpGet("strike/mode")]
      public async Task<IActionResult> GetStrikeModeAsync()
      {
            var res = await door.GetStrikeModeAsync();
            return Ok(res);
      }

      [HttpGet("apb/mode")]
      public async Task<IActionResult> GetApbModeAsync()
      {
            var res = await door.GetApbModeAsync();
            return Ok(res);
      }

      [HttpGet("mode")]
      public async Task<IActionResult> GetDoorModeAsync()
      {
            var res = await door.GetDoorModeAsync();
            return Ok(res);
      }

      [HttpGet("acsflag")]
      public async Task<IActionResult> GetAccessControlFlagAsync()
      {
            var res = await door.GetAccessControlFlagAsync();
            return Ok(res);
      }

      [HttpGet("spareflag")]
      public async Task<IActionResult> GetSpareFlagAsync()
      {
            var res = await door.GetSpareFlagAsync();
            return Ok(res);
      }

      [HttpGet("osdp/baudrate")]
      public async Task<IActionResult> GetOsdpBaudrateAsync()
      {
            var res = await door.GetOsdpBaudrateAsync();
            return Ok(res);
      }

      [HttpGet("option/{LocationId}")]
      public async Task<IActionResult> GetDoorOptionByLocationIdAsync(int LocationId)
      {
            var res = await door.GetDoorOptionByLocationIdAsync(LocationId);
            return Ok(res);
      }

}