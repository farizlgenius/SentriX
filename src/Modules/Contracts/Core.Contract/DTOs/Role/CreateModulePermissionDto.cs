namespace Core.Contract.DTOs.Role;

public sealed record CreateModulePermissionDto(
      Guid Guid,
      bool IsEnabled,
      List<CreateFeaturePermissionDto> FeaturePermission
);