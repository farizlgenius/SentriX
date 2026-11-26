using Core.Contract.DTOs.Role;

namespace Core.Contract.Interfaces;

public interface IRole : IService<RoleDto, CreateRoleDto, UpdateRoleDto>
{
  Task<IEnumerable<RoleDto>> GetAsync(CancellationToken ct = default);
  Task<IEnumerable<ModuleDto>> GetRoleModuleAsync(CancellationToken ct = default);
}