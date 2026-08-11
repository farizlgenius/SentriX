namespace Core.Contract.DTOs.Role;

public sealed record CreateRoleDto(
      string Name,
      List<PermissionDto> Permissions,
      Guid LocationGuid
);