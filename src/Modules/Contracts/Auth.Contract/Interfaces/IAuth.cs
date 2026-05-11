using System;
using Auth.Contract.DTOs;
using SharedKernel.Domain;

namespace Auth.Contract.Interfaces;

public interface IAuth
{
      Task<AccessTokenDto> LoginAsync(LoginDto login);
      Task<AccessTokenDto> RefreshTokenAsync(string refreshToken);
      Task<BaseResponse> LogoutAsync(string refreshToken);
      Task<MeDto> GetMeByUsernameAndRoleIdAsync(string username, int roleId);
}
