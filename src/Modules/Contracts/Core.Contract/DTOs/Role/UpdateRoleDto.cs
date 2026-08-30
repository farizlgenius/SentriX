namespace Core.Contract.DTOs.Role;

public sealed record UpdateRoleDto(
      Guid Guid,
      string Name,
      List<CreateModulePermissionDto> Modules
);