using System;

namespace Role.Contract.DTOs;

public sealed record CreateRoleDto(string Name, List<PermissionDto> Permissions, int LocationId);
