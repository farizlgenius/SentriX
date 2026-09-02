using Core.Application.Interfaces;
using Core.Contract.Queries;
using SharedKernel.Messaging;

namespace Core.Application.Handler;

public sealed class IsValidRoleByGuidQueryHandler(IRoleRepository repo) : IQueryHandler<IsValidRoleByGuidQuery, bool>
{
  public async Task<bool> HandleAsync(IsValidRoleByGuidQuery query, CancellationToken ct)
  {
    return await repo.IsAnyGuidAsync(query.RoleGuid, ct);
  }
}