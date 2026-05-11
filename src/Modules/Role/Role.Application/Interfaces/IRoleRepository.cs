using System;
using Role.Contract.DTOs;

namespace Role.Application.Interfaces;

public interface IRoleRepository
{
      Task<List<PermissionDto>> GetPermissionsByRoleIdAsync(int RoleId, CancellationToken ct = default);

}
