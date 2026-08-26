namespace Core.Contract.DTOs.Role;

public sealed record CreateFeaturePermissionDto(
      Guid Guid,
      bool IsEnabled,
      bool IsCreated,
      bool IsUpdated,
      bool IsDeleted
);