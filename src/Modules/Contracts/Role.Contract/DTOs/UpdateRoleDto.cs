using System;

namespace Role.Contract.DTOs;

public sealed record UpdateRoleDto(int Id, string Name, List<PermissionDto> Permissions, int LocationId);