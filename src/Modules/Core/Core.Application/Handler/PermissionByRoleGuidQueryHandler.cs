using Core.Application.Interfaces;
using Core.Contract.DTOs.Role;
using Core.Contract.Queries;
using SharedKernel.Messaging;

namespace Core.Application.Handler;

public sealed class PermissionByRoleGuidQueryHandler(IRoleRepository repo) : IQueryHandler<PermissionByRoleGuidQuery, IEnumerable<ModulePermissionDto>>
{
  public async Task<IEnumerable<ModulePermissionDto>> HandleAsync(PermissionByRoleGuidQuery query, CancellationToken ct)
  {
    var id = await repo.GetIdByGuidAsync(query.roleGuid);
    return await repo.GetPermissionByRoleIdAsync(id, ct);
  }
}