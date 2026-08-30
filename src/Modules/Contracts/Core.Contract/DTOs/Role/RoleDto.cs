namespace Core.Contract.DTOs.Role;

public sealed record RoleDto(
      Guid Guid,
      string Name,
      List<ModulePermissionDto> Modules,
      bool IsActive,
      bool IsDefault
);