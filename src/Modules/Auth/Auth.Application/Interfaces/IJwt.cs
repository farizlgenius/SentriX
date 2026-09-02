using System;
using Auth.Contract.DTOs;
using Core.Contract.DTOs.Operator;
using Core.Contract.DTOs.User;

namespace Auth.Application.Interfaces;

public interface IJwt
{
      Task<AccessTokenDto> GenerateTokenAsync(OperatorDto user);
      Task<AccessTokenDto> RefreshTokenAsync(OperatorDto refreshToken);
      Task<bool> RevokeTokenAsync(string refreshToken);
      Task<RefreshTokenDto> GetRefreshTokenAsync(string hashed);
}
