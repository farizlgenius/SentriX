using System;
using Role.Application.Interfaces;
using Role.Contract.Queries;
using SharedKernel.Messaging;

namespace Role.Application.Queries;

public sealed class IsValidRoleIdQueryHandler(IRoleRepository repo) : IQueryHandler<IsValidRoleIdQuery, bool>
{
      public async Task<bool> HandleAsync(IsValidRoleIdQuery query, CancellationToken ct)
      {
            return await repo.IsValidRoleIdAsync(query.RoleId);
      }
}
