using Core.Contract.DTOs.Role;
using Core.Domain.Entities;

namespace Core.Application.Interfaces;

public interface IRoleRepository : IBaseRepository<RoleDto, Role>
{
  Task<bool> IsAnyUserByGuidAsync(Guid guid, CancellationToken ct = default);
  Task<IEnumerable<ModulePermissionDto>> GetPermissionByRoleIdAsync(int id, CancellationToken ct = default);
}