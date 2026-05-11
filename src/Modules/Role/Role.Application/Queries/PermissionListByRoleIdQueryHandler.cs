using System;
using Role.Application.Interfaces;
using Role.Contract.DTOs;
using Role.Contract.Queries;
using SharedKernel.Messaging;

namespace Role.Application.Queries;


public sealed class PermissionListByRoleIdQueryHandler(IRoleRepository repo) : IQueryHandler<PermissionListByRoleIdQuery, List<PermissionDto>>
{
      public async Task<List<PermissionDto>> HandleAsync(PermissionListByRoleIdQuery query, CancellationToken ct)
      {
            return await repo.GetPermissionsByRoleIdAsync(query.RoleId, ct);
      }
}
