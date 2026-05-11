using System;

namespace Role.Contract.DTOs;


public sealed record RoleDto(int Id, string Name, List<PermissionDto> Permissions, string LocationName);
