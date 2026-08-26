namespace Core.Contract.DTOs.Role;

public sealed record FeaturePermissionDto(
      Guid Guid,
      bool IsEnabled,
      bool IsCreated,
      bool IsUpdated,
      bool IsDeleted
);