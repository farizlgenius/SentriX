using Core.Contract.DTOs.Role;

namespace Core.Contract.Interfaces;

public interface IRole : IBase<RoleDto, CreateRoleDto, UpdateRoleDto>
{
  Task<IEnumerable<RoleDto>> GetAsync(CancellationToken ct = default);
}