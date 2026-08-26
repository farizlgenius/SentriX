namespace Core.Contract.DTOs.Role;

public sealed record FeaturePermissionDto(
      string Name,
      bool IsEnabled,
      bool IsCreated,
      bool IsUpdated,
      bool IsDeleted
);