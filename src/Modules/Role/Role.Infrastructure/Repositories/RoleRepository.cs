using System;
using Microsoft.EntityFrameworkCore;
using Role.Application.Interfaces;
using Role.Contract.DTOs;
using Role.Infrastructure.Persistences;

namespace Role.Infrastructure.Repositories;

public sealed class RoleRepository(RoleDbContext context) : IRoleRepository
{
      public async Task<List<PermissionDto>> GetPermissionsByRoleIdAsync(int RoleId, CancellationToken ct = default)
      {
            return await context.permissions.AsNoTracking()
                  .Where(p => p.role_id == RoleId)
                  .Select(p => new PermissionDto(
                        p.feature_id,
                        p.feature.name,
                        p.is_enabled,
                        p.is_created,
                        p.is_updated,
                        p.is_deleted
                  ))
                  .ToListAsync(ct);
      }
}
