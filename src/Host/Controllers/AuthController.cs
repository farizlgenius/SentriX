using System.Security.Claims;
using System.Threading.Channels;
using Auth.Contract.DTOs;
using Auth.Contract.Interfaces;
using Core.Contract.DTOs.Events.Audit;
using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace Host.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController(
    IAuth auth
    ) : ControllerBase
  {
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromForm] LoginDto login)
    {
      var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();

      var res = await auth.LoginAsync(login,clientIp ?? string.Empty);

      Response.Cookies.Append("refresh_token", res.RefreshToken, new CookieOptions
      {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.None,
        // Path = "/api/Auth",
        Expires = new DateTimeOffset(res.RefreshExpiredAt, TimeSpan.Zero)
      });


      return Ok(res);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync([FromBody] RefreshDto dto)
    {

      if (string.IsNullOrWhiteSpace(dto.Refresh))
      {
        string? refreshToken;
        Request.Cookies.TryGetValue("refresh_token", out refreshToken);
        var result = await auth.RefreshTokenAsync(refreshToken ?? "");
        Response.Cookies.Append("refresh_token", result.RefreshToken, new CookieOptions
        {
          HttpOnly = true,
          Secure = true,
          SameSite = SameSiteMode.None,
          // Path = "/api/Auth",
          Expires = new DateTimeOffset(result.RefreshExpiredAt, TimeSpan.Zero)
        });
        return Ok(result);
      }
      else
      {
        var result = await auth.RefreshTokenAsync(dto.Refresh);
        Response.Cookies.Append("refresh_token", result.RefreshToken, new CookieOptions
        {
          HttpOnly = true,
          Secure = true,
          SameSite = SameSiteMode.None,
          // Path = "/api/Auth",
          Expires = new DateTimeOffset(result.RefreshExpiredAt, TimeSpan.Zero)
        });
        return Ok(result);
      }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] RefreshDto dto)
    {
      var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

      var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();

      if (string.IsNullOrWhiteSpace(dto.Refresh))
      {
        string? refreshToken;
        Request.Cookies.TryGetValue("refresh_token", out refreshToken);
        await auth.LogoutAsync(refreshToken ?? "",username,clientIp ?? string.Empty);
        Response.Cookies.Delete("refresh_token", new CookieOptions
        {
          HttpOnly = true,
          Secure = true,
          SameSite = SameSiteMode.None,
          // Path = "/api/Auth"
        });
        return Ok();
      }
      else
      {
        await auth.LogoutAsync(dto.Refresh,username,clientIp ?? string.Empty);
        Response.Cookies.Delete("refresh_token", new CookieOptions
        {
          HttpOnly = true,
          Secure = true,
          SameSite = SameSiteMode.None,
          // Path = "/api/Auth"
        });
        return Ok();
      }
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMe()
    {
      var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
      var roleGuid = User.FindFirst("role_guid")?.Value ?? "";

      

      var result = await auth.GetMeByUsernameAndRoleGuidAsync(username, new Guid(roleGuid));
      return Ok(result);

    }
  }
}
