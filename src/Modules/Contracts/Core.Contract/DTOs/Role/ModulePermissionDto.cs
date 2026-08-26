namespace Core.Contract.DTOs.Role;

public sealed record ModulePermissionDto(
      string Name,
      bool IsEnabled,
      List<FeaturePermissionDto> FeaturePermission
);