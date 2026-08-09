// using Microsoft.AspNetCore.Mvc;
// using SharedKernel.Domain;
// using Storage.Contract.Interfaces;

// namespace Host.Controllers;

// [Route("api/[controller]")]
// [ApiController]
// public class UserController(IUser user) : ControllerBase
// {
//       // User
//       [HttpGet("pagination")]
//       public async Task<IActionResult> GetUserPaginationAsync([FromQuery] PaginationParams param)
//       {
//             var res = await user.GetUserPaginationAsync(param);
//             return Ok(res);
//       }

//       [HttpPost]
//       public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserDto dto)
//       {
//             var res = await user.CreateUserAsync(dto);
//             return Ok(res);
//       }

//       [HttpPut]
//       public async Task<IActionResult> UpdateUserAsync([FromBody] UserDto dto)
//       {
//             var res = await user.UpdateUserAsync(dto);
//             return Ok(res);
//       }

//       [HttpDelete("{guid}")]
//       public async Task<IActionResult> DeleteUserAsync(Guid guid)
//       {
//             var res = await user.DeleteUserAsync(guid);
//             return Ok(res);
//       }

//       [HttpGet("image/{userid}")]
//       [Produces("image/png")]
//       public async Task<IActionResult> GetImageAsync(string userid)
//       {
//             var stream = await user.GetImageByUserIdAsync(userid);
//             return File(stream, "image/png");
//       }

//       [HttpPost("image/upload/{userid}")]
//       [Consumes("multipart/form-data")]
//       public async Task<IActionResult> UploadImageAsync([FromForm] UploadImageDto request, string userid)
//       {
//             await user.UploadImageAsync(userid, request.Image.OpenReadStream());
//             return Ok();
//       }

//       // Company
//       [HttpGet("/api/company/option/{location}")]
//       public async Task<IActionResult> GetCompanyOptionByLocationAsync(int location)
//       {
//             var res = await user.GetCompanyOptionByLocationIdAsync(location);
//             return Ok(res);
//       }

//       [HttpGet("/api/company/pagination")]
//       public async Task<IActionResult> GetCompanyPaginationAsync([FromQuery] PaginationParams param)
//       {
//             var res = await user.GetCompanyPaginationAsync(param);
//             return Ok(res);
//       }

//       [HttpGet("/api/company")]
//       public async Task<IActionResult> GetCompanyByLocationIdAsync([FromQuery]int LocationId)
//       {
//             var res = await user.GetCompanyByLocationIdAsync(LocationId);
//             return Ok(res);
//       }

//       [HttpPost("/api/company")]
//       public async Task<IActionResult> CreateCompanyAsync([FromBody] CreateCompanyDto dto)
//       {
//             var res = await user.CreateCompanyAsync(dto);
//             return Ok(res);
//       }

//       [HttpPut("/api/company")]
//       public async Task<IActionResult> UpdateCompanyAsync([FromBody] CompanyDto dto)
//       {
//             var res = await user.UpdateCompanyAsync(dto);
//             return Ok(res);
//       }

//       [HttpDelete("/api/company/{guid}")]
//       public async Task<IActionResult> DeleteCompanyAsync(Guid guid)
//       {
//             var res = await user.DeleteCompanyAsync(guid);
//             return Ok(res);
//       }


//       // Department
//       [HttpGet("/api/department/pagination")]
//       public async Task<IActionResult> GetDepartmentPaginationAsync([FromQuery] PaginationParams param)
//       {
//             var res = await user.GetDepartmentPaginationAsync(param);
//             return Ok(res);
//       }

//       [HttpGet("/api/department/pagination/{guid}")]
//       public async Task<IActionResult> GetDepartmentByCompanyGuidAsync(Guid guid,[FromQuery] PaginationParams param)
//       {
//             var res = await user.GetDepartmentPaginationByCompanyGuidAsync(param,guid);
//             return Ok(res);            
//       }

//       [HttpGet("/api/department/company/{guid}")]
//       public async Task<IActionResult> GetDepartmentByCompanyAsync(Guid guid)
//       {
//             var res = await user.GetDepartmentByCompanyGuidAsync(guid);
//             return Ok(res);
//       }

//       [HttpGet("/api/department/option/company/{guid}")]
//       public async Task<IActionResult> GetDepartmentOptionByCompanyAsync(Guid guid)
//       {
//             var res = await user.GetDepartmentOptionByCompanyGuidAsync(guid);
//             return Ok(res);
//       }

//       [HttpPost("/api/department")]
//       public async Task<IActionResult> CreateDepartmentAsync([FromBody] CreateDepartmentDto dto)
//       {
//             var res = await user.CreateDepartmentAsync(dto);
//             return Ok(res);
//       }

//       [HttpPut("/api/department")]
//       public async Task<IActionResult> UpdateDepartmentAsync([FromBody] DepartmentDto dto)
//       {
//             var res = await user.UpdateDepartmentAsync(dto);
//             return Ok(res);
//       }

//       [HttpDelete("/api/department/{guid}")]
//       public async Task<IActionResult> DeleteDepartmentAsync(Guid guid)
//       {
//             var res = await user.DeleteDepartmentAsync(guid);
//             return Ok(res);
//       }

//       // Position
//       [HttpGet("/api/position/pagination")]
//       public async Task<IActionResult> GetPositionPaginationAsync([FromQuery] PaginationParams param)
//       {
//             var res = await user.GetPositionPaginationAsync(param);
//             return Ok(res);
//       }

//       [HttpGet("/api/position/pagination/{guid}")]
//       public async Task<IActionResult> GetPositionByDepartmentAsync(Guid guid,[FromQuery] PaginationParams param)
//       {
//             var res = await user.GetPositionPaginationByDepartmentGuidAsync(param,guid);
//             return Ok(res);
//       }

//       [HttpPost("/api/position")]
//       public async Task<IActionResult> CreatePositionAsync([FromBody] CreatePositionDto dto)
//       {
//             var res = await user.CreatePositionAsync(dto);
//             return Ok(res);
//       }

//       [HttpPut("/api/position")]
//       public async Task<IActionResult> UpdatePositionAsync([FromBody] PositionDto dto)
//       {
//             var res = await user.UpdatePositionAsync(dto);
//             return Ok(res);
//       }

//       [HttpDelete("/api/position/{guid}")]
//       public async Task<IActionResult> DeletePositionAsync(Guid guid)
//       {
//             var res = await user.DeletePositionAsync(guid);
//             return Ok(res);
//       }

//       [HttpGet("/api/position/option/department/{guid}")]
//       public async Task<IActionResult> GetPositionOptionByDepartmentAsync(Guid guid)
//       {
//             var res = await user.GetPositionOptionByDepartmentGuidAsync(guid);
//             return Ok(res);
//       }

//       [HttpGet("flag")]
//       public async Task<IActionResult> GetUserFlagOptionAsync()
//       {
//             var res = await user.GetUserFlagOptionAsync();
//             return Ok(res);
//       }

//       [HttpGet("scan")]
//       public async Task<IActionResult> ScanUserAsync()
//       {
//             // var res = await user.ScanUserAsync();
//             // return Ok(res);
//             return Ok();
//       }
// }