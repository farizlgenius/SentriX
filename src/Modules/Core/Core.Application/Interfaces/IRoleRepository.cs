using Core.Contract.DTOs.Role;
using Core.Domain.Entities;

namespace Core.Application.Interfaces;

public interface IRoleRepository : IBaseRepository<RoleDto,Role>
{
      Task<bool> IsAnyNameAsync(string Name,Guid locationGuid,CancellationToken ct = default);
}