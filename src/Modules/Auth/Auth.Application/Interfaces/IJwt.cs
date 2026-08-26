using System;
using Auth.Contract.DTOs;
using Core.Contract.DTOs.User;

namespace Auth.Application.Interfaces;

public interface IJwt
{
      Task<AccessTokenDto> GenerateTokenAsync(UserDto user);
      Task<AccessTokenDto> RefreshTokenAsync(UserDto refreshToken);
      Task<bool> RevokeTokenAsync(string refreshToken);
      Task<RefreshTokenDto> GetRefreshTokenAsync(string hashed);
}
