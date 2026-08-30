namespace Core.Contract.DTOs.Role;

public sealed record CreateModulePermissionDto(
      int Id,
      bool IsEnabled,
      List<CreateFeaturePermissionDto> Features
);