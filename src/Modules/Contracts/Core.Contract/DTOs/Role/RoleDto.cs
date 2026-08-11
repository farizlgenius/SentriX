namespace Core.Contract.DTOs.Role;

public sealed record RoleDto(
      Guid Guid,
      string Name,
      List<PermissionDto> Permissions,
      bool IsActive,
      bool IsDefault
);