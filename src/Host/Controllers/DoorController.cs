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

}