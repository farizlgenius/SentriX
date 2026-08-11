using Core.Contract.DTOs.Role;
using Core.Domain.Entities;

namespace Core.Application.Interfaces;

public interface IRoleRepository : IBaseRepository<RoleDto, Role>
{
  Task<bool> IsAnyOperatorAsync(Guid guid, CancellationToken ct = default);
}