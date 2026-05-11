using System.Security.Claims;
using Auth.Contract.DTOs;
using Auth.Contract.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuth auth) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromForm] LoginDto login)
        {
            var res = await auth.LoginAsync(login);

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

            if (string.IsNullOrWhiteSpace(dto.Refresh))
            {
                string? refreshToken;
                Request.Cookies.TryGetValue("refresh_token", out refreshToken);
                var result = await auth.LogoutAsync(refreshToken ?? "");
                Response.Cookies.Delete("refresh_token", new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    // Path = "/api/Auth"
                });
                return Ok(result);
            }
            else
            {
                var result = await auth.LogoutAsync(dto.Refresh);
                Response.Cookies.Delete("refresh_token", new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    // Path = "/api/Auth"
                });
                return Ok(result);
            }
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var role_id = User.FindFirst("role_id")?.Value ?? "";

            var result = await auth.GetMeByUsernameAndRoleIdAsync(username, int.Parse(role_id));
            return Ok(result);

        }
    }
}
