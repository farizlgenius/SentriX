namespace Core.Contract.DTOs.Role;

public sealed record CreateRoleDto(
      string Name,
      List<CreateModulePermissionDto> ModulePermissions
);