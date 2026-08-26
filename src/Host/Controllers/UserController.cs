using Core.Contract.DTOs.User;
using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;
using Storage.Contract.Interfaces;

namespace Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUser user) : ControllerBase
{
  // User
  [HttpGet("pagination")]
  public async Task<IActionResult> GetUserPaginationAsync([FromQuery] PaginationParams param)
  {
    var res = await user.GetPaginationAsync(param);
    return Ok(res);
  }

  [HttpPatch]
  public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordDto dto)
  {
    var res = await user.ChangePasswordAsync(dto);
    return Ok(res);
  }

  [HttpPost]
  public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserDto dto)
  {
    var res = await user.CreateAsync(dto);
    return Ok(res);
  }

  [HttpPut]
  public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserDto dto)
  {
    var res = await user.UpdateAsync(dto);
    return Ok(res);
  }

  [HttpDelete("{guid}")]
  public async Task<IActionResult> DeleteUserAsync(Guid guid)
  {
    var res = await user.DeleteByGuidAsync(guid);
    return Ok(res);
  }

  // [HttpGet("image/{userid}")]
  // [Produces("image/png")]
  // public async Task<IActionResult> GetImageAsync(string userid)
  // {
  //   var stream = await user.GetImageByUserIdAsync(userid);
  //   return File(stream, "image/png");
  // }

  // [HttpPost("image/upload/{userid}")]
  // [Consumes("multipart/form-data")]
  // public async Task<IActionResult> UploadImageAsync([FromForm] UploadImageDto request, string userid)
  // {
  //   await user.UploadImageAsync(userid, request.Image.OpenReadStream());
  //   return Ok();
  // }



  // [HttpGet("flag")]
  // public async Task<IActionResult> GetUserFlagOptionAsync()
  // {
  //   var res = await user.GetUserFlagOptionAsync();
  //   return Ok(res);
  // }

  // [HttpGet("scan")]
  // public async Task<IActionResult> ScanUserAsync()
  // {
  //   // var res = await user.ScanUserAsync();
  //   // return Ok(res);
  //   return Ok();
  // }
}