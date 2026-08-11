namespace Core.Contract.DTOs.Role;

public sealed record PermissionDto(
      Guid Guid,
      Guid FeatureGuid,
      Guid RoleGuid,
      bool IsEnabled,
      bool IsCreated,
      bool IsUpdated,
      bool IsDeleted
);