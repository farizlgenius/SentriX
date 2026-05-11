using System;
using Auth.Contract.DTOs;

namespace Auth.Application.Interfaces;

public interface IRefreshTokenAuditRepository
{
      Task AddAsync(string username, string hashedRefreshToken, string action, DateTime expiredAt);
      Task<RefreshTokenDto> GetRefreshTokenAsync(string hash);

}
