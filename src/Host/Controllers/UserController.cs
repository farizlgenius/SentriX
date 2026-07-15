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
            await user.UploadImageAsync(userid, request.Image.OpenReadStream());
            return Ok();
      }

      // Company
      [HttpGet("/api/company/option/{location}")]
      public async Task<IActionResult> GetCompanyOptionByLocationAsync(int location)
      {
            var res = await user.GetCompanyOptionByLocationAsync(location);
            return Ok(res);
      }

      [HttpGet("/api/company/pagination")]
      public async Task<IActionResult> GetCompanyPaginationAsync([FromQuery] PaginationParams param)
      {
            var res = await user.GetCompanyPaginationAsync(param);
            return Ok(res);
      }

      [HttpGet("/api/company")]
      public async Task<IActionResult> GetCompanyByLocationIdAsync([FromQuery]int LocationId)
      {
            var res = await user.GetCompanyByLocationIdAsync(LocationId);
            return Ok(res);
      }

      [HttpPost("/api/company")]
      public async Task<IActionResult> CreateCompanyAsync([FromBody] CreateCompanyDto dto)
      {
            var res = await user.CreateCompanyAsync(dto);
            return Ok(res);
      }

      [HttpPut("/api/company")]
      public async Task<IActionResult> UpdateCompanyAsync([FromBody] CompanyDto dto)
      {
            var res = await user.UpdateCompanyAsync(dto);
            return Ok(res);
      }

      [HttpDelete("/api/company/{id}")]
      public async Task<IActionResult> DeleteCompanyAsync(int id)
      {
            var res = await user.DeleteCompanyAsync(id);
            return Ok(res);
      }


      // Department
      [HttpGet("/api/department/pagination")]
      public async Task<IActionResult> GetDepartmentPaginationAsync([FromQuery] PaginationParams param)
      {
            var res = await user.GetDepartmentPaginationAsync(param);
            return Ok(res);
      }

      [HttpGet("/api/department/pagination/{companyId}")]
      public async Task<IActionResult> GetDepartmentByCompanyAsync(int companyId,[FromQuery] PaginationParams param)
      {
            var res = await user.GetDepartmentByCompanyAsync(param,companyId);
            return Ok(res);            
      }

      [HttpGet("/api/department/company/{companyId}")]
      public async Task<IActionResult> GetDepartmentByCompanyAsync(int companyId)
      {
            var res = await user.GetDepartmentByCompanyAsync(companyId);
            return Ok(res);
      }

      [HttpGet("/api/department/option/company/{companyId}")]
      public async Task<IActionResult> GetDepartmentOptionByCompanyAsync(int companyId)
      {
            var res = await user.GetDepartmentOptionByCompanyAsync(companyId);
            return Ok(res);
      }

      [HttpPost("/api/department")]
      public async Task<IActionResult> CreateDepartmentAsync([FromBody] CreateDepartmentDto dto)
      {
            var res = await user.CreateDepartmentAsync(dto);
            return Ok(res);
      }

      [HttpPut("/api/department")]
      public async Task<IActionResult> UpdateDepartmentAsync([FromBody] DepartmentDto dto)
      {
            var res = await user.UpdateDepartmentAsync(dto);
            return Ok(res);
      }

      [HttpDelete("/api/department/{id}")]
      public async Task<IActionResult> DeleteDepartmentAsync(int id)
      {
            var res = await user.DeleteDepartmentAsync(id);
            return Ok(res);
      }

      // Position
      [HttpGet("/api/position/pagination")]
      public async Task<IActionResult> GetPositionPaginationAsync([FromQuery] PaginationParams param)
      {
            var res = await user.GetPositionPaginationAsync(param);
            return Ok(res);
      }

      [HttpGet("/api/position/pagination/{departmentId}")]
      public async Task<IActionResult> GetPositionByDepartmentAsync(int departmentId,[FromQuery] PaginationParams param)
      {
            var res = await user.GetPositionByDepartmentAsync(param,departmentId);
            return Ok(res);
      }

      [HttpPost("/api/position")]
      public async Task<IActionResult> CreatePositionAsync([FromBody] CreatePositionDto dto)
      {
            var res = await user.CreatePositionAsync(dto);
            return Ok(res);
      }

      [HttpPut("/api/position")]
      public async Task<IActionResult> UpdatePositionAsync([FromBody] PositionDto dto)
      {
            var res = await user.UpdatePositionAsync(dto);
            return Ok(res);
      }

      [HttpDelete("/api/position/{id}")]
      public async Task<IActionResult> DeletePositionAsync(int id)
      {
            var res = await user.DeletePositionAsync(id);
            return Ok(res);
      }

      [HttpGet("/api/position/option/department/{deprtmentId}")]
      public async Task<IActionResult> GetPositionOptionByDepartmentAsync(int deprtmentId)
      {
            var res = await user.GetPositionOptionByDepartmentAsync(deprtmentId);
            return Ok(res);
      }

      [HttpGet("flag")]
      public async Task<IActionResult> GetUserFlagOptionAsync()
      {
            var res = await user.GetUserFlagOptionAsync();
            return Ok(res);
      }

      [HttpGet("scan")]
      public async Task<IActionResult> ScanUserAsync()
      {
            // var res = await user.ScanUserAsync();
            // return Ok(res);
            return Ok();
      }
}