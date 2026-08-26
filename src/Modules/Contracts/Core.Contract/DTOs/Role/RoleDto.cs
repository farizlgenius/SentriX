namespace Core.Contract.DTOs.Role;

public sealed record RoleDto(
      Guid Guid,
      string Name,
      List<ModulePermissionDto> ModulePermissionDtos,
      bool IsActive,
      bool IsDefault
);