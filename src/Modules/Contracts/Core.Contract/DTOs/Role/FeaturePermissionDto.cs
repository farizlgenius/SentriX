namespace Core.Contract.DTOs.Role;

public sealed record FeaturePermissionDto(
      int Id,
      string Name,
      bool IsEnabled,
      bool IsCreated,
      bool IsUpdated,
      bool IsDeleted
);