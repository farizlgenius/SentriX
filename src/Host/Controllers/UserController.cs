using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;
using Storage.Contract.Interfaces;
using User.Contract.DTOs;
using User.Contract.Interfaces;

namespace Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUser user) : ControllerBase
{
      // User
      [HttpGet("pagination")]
      public async Task<IActionResult> GetUserPaginationAsync([FromQuery] PaginationParams param)
      {
            var res = await user.GetUserPaginationAsync(param);
            return Ok(res);
      }

      [HttpPost]
      public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserDto dto)
      {
            var res = await user.CreateUserAsync(dto);
            return Ok(res);
      }

      [HttpPut]
      public async Task<IActionResult> UpdateUserAsync([FromBody] UserDto dto)
      {
            var res = await user.UpdateUserAsync(dto);
            return Ok(res);
      }

      [HttpDelete("{id}")]
      public async Task<IActionResult> DeleteUserAsync(int id)
      {
            var res = await user.DeleteUserAsync(id);
            return Ok(res);
      }

      [HttpGet("image/{userid}")]
      [Produces("image/png")]
      public async Task<IActionResult> GetImageAsync(string userid)
      {
            var stream = await user.GetImageByUserIdAsync(userid);
            return File(stream, "image/png");
      }

      [HttpPost("image/upload/{userid}")]
      [Consumes("multipart/form-data")]
      public async Task<IActionResult> UploadImageAsync([FromForm] UploadImageDto request, string userid)
      {
            var res = await user.UploadImageAsync(userid, request.Image.OpenReadStream());
            return Ok(res);
      }

      // Company
      [HttpGet("company/pagination")]
      public async Task<IActionResult> GetCompanyPaginationAsync([FromQuery] PaginationParams param)
      {
            var res = await user.GetCompanyPaginationAsync(param);
            return Ok(res);
      }

      [HttpPost("company")]
      public async Task<IActionResult> CreateCompanyAsync([FromBody] CreateCompanyDto dto)
      {
            var res = await user.CreateCompanyAsync(dto);
            return Ok(res);
      }

      [HttpPut("company")]
      public async Task<IActionResult> UpdateCompanyAsync([FromBody] CompanyDto dto)
      {
            var res = await user.UpdateCompanyAsync(dto);
            return Ok(res);
      }

      [HttpDelete("company/{id}")]
      public async Task<IActionResult> DeleteCompanyAsync(int id)
      {
            var res = await user.DeleteCompanyAsync(id);
            return Ok(res);
      }

      // Department
      [HttpGet("department/pagination")]
      public async Task<IActionResult> GetDepartmentPaginationAsync([FromQuery] PaginationParams param)
      {
            var res = await user.GetDepartmentPaginationAsync(param);
            return Ok(res);
      }

      [HttpPost("department")]
      public async Task<IActionResult> CreateDepartmentAsync([FromBody] CreateDepartmentDto dto)
      {
            var res = await user.CreateDepartmentAsync(dto);
            return Ok(res);
      }

      [HttpPut("department")]
      public async Task<IActionResult> UpdateDepartmentAsync([FromBody] DepartmentDto dto)
      {
            var res = await user.UpdateDepartmentAsync(dto);
            return Ok(res);
      }

      [HttpDelete("department/{id}")]
      public async Task<IActionResult> DeleteDepartmentAsync(int id)
      {
            var res = await user.DeleteDepartmentAsync(id);
            return Ok(res);
      }

      // Position
      [HttpGet("position/pagination")]
      public async Task<IActionResult> GetPositionPaginationAsync([FromQuery] PaginationParams param)
      {
            var res = await user.GetPositionPaginationAsync(param);
            return Ok(res);
      }

      [HttpPost("position")]
      public async Task<IActionResult> CreatePositionAsync([FromBody] CreatePositionDto dto)
      {
            var res = await user.CreatePositionAsync(dto);
            return Ok(res);
      }

      [HttpPut("position")]
      public async Task<IActionResult> UpdatePositionAsync([FromBody] PositionDto dto)
      {
            var res = await user.UpdatePositionAsync(dto);
            return Ok(res);
      }

      [HttpDelete("position/{id}")]
      public async Task<IActionResult> DeletePositionAsync(int id)
      {
            var res = await user.DeletePositionAsync(id);
            return Ok(res);
      }
}