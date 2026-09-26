using System;
using Auth.Contract.DTOs;
using SharedKernel.Domain;

namespace Auth.Contract.Interfaces;

public interface IAuth
{
      Task<AccessTokenDto> LoginAsync(LoginDto login,string ip);
      Task<AccessTokenDto> RefreshTokenAsync(string refreshToken);
      Task<string> LogoutAsync(string refreshToken,string username,string ip);
      Task<MeDto> GetMeByUsernameAndRoleGuidAsync(string username, Guid roleGuid);
}
