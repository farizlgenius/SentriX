using Core.Application.Interfaces;
using Core.Contract.DTOs.Role;
using Core.Contract.Queries;
using SharedKernel.Messaging;

namespace Core.Application.Handler;

public sealed class PermissionByRoleGuidQueryHandler(IRoleRepository repo) : IQueryHandler<PermissionByRoleGuidQuery, IEnumerable<PermissionDto>>
{
  public async Task<IEnumerable<PermissionDto>> HandleAsync(PermissionByRoleGuidQuery query, CancellationToken ct)
  {
    return await repo.GetPermissionByRoleGuidAsync(query.roleGuid, ct);
  }
}