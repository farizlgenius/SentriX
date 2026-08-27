namespace Core.Contract.DTOs.Role;

public sealed record ModulePermissionDto(
      int Id,
      string Name,
      bool IsEnabled,
      List<FeaturePermissionDto> FeaturePermission
);