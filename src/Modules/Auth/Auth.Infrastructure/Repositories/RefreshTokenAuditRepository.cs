using System;
using Auth.Application.Interfaces;
using Auth.Contract.DTOs;
using Auth.Domain.Enums;
using Auth.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Repositories;

public sealed class RefreshTokenAuditRepository(AuthDbContext context) : IRefreshTokenAuditRepository
{
      public async Task AddAsync(string username, string hashedRefreshToken, string action, DateTime expiredAt)
      {
            var audit = new Persistence.Entities.RefreshTokenAudit
            {
                  username = username,
                  hashed_refresh_token = hashedRefreshToken,
                  action = action,
                  expired_at = expiredAt
            };

            await context.RefreshTokenAudits.AddAsync(audit);
            await context.SaveChangesAsync();
      }

      public async Task<RefreshTokenDto> GetRefreshTokenAsync(string hash)
      {
            return await context.RefreshTokenAudits
            .AsNoTracking()
            .OrderBy(x => x.id)
            .Where(x => x.hashed_refresh_token.Equals(hash))
            .Select(x => new RefreshTokenDto(x.hashed_refresh_token, x.username, x.action, x.expired_at))
            .FirstOrDefaultAsync() ??
            new RefreshTokenDto(string.Empty, string.Empty, TokenAction.CREATE.ToString(), DateTime.UtcNow)
            ;
      }
}
