using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;
using Storage.Contract.Interfaces;
using User.Contract.DTOs;

namespace Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IStorage file) : ControllerBase
{
      // User
      [HttpGet("pagination")]
      public async Task<IActionResult> GetUserPaginationAsync([FromQuery] PaginationParams param)
      {
            return Ok();
      }

      [HttpPost]
      public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserDto dto)
      {
            return Ok();
      }

      [HttpPut]
      public async Task<IActionResult> UpdateUserAsync([FromBody] UserDto dto)
      {
            return Ok();
      }

      [HttpDelete("{id}")]
      public async Task<IActionResult> DeleteUserAsync(int id)
      {
            return Ok();
      }

      [HttpGet("image/{userid}")]
        [Produces("image/png")]
        public async Task<IActionResult> GetImageAsync(string userid)
        {
            if (string.IsNullOrEmpty(userid)) return BadRequest();
            var stream = await file.ReadUserAsync(userid);

            return File(stream, "image/png");
        }

      // Company
      [HttpGet("company/pagination")]
      public async Task<IActionResult> GetCompanyPaginationAsync([FromQuery] PaginationParams param)
      {
            return Ok();
      }

      [HttpPost("company")]
      public async Task<IActionResult> CreateCompanyAsync()
      {
            return Ok();
      }

      [HttpPut("company")]
      public async Task<IActionResult> UpdateCompanyAsync()
      {
            return Ok();
      }

      [HttpDelete("company/{id}")]
      public async Task<IActionResult> DeleteCompanyAsync(int id)
      {
            return Ok();
      }

      // Department
      [HttpGet("department/pagination")]
      public async Task<IActionResult> GetDepartmentPaginationAsync([FromQuery] PaginationParams param)
      {
            return Ok();
      }

      [HttpPost("department")]
      public async Task<IActionResult> CreateDepartmentAsync()
      {
            return Ok();
      }

      [HttpPut("department")]
      public async Task<IActionResult> UpdateDepartmentAsync()
      {
            return Ok();
      }

      [HttpDelete("department/{id}")]
      public async Task<IActionResult> DeleteDepartmentAsync(int id)
      {
            return Ok();
      }

      // Position
      [HttpGet("position/pagination")]
      public async Task<IActionResult> GetPositionPaginationAsync([FromQuery] PaginationParams param)
      {
            return Ok();
      }

      [HttpPost("position")]
      public async Task<IActionResult> CreatePositionAsync()
      {
            return Ok();
      }

      [HttpPut("position")]
      public async Task<IActionResult> UpdatePositionAsync()
      {
            return Ok();
      }

      [HttpDelete("position/{id}")]
      public async Task<IActionResult> DeletePositionAsync(int id)
      {
            return Ok();
      }
}