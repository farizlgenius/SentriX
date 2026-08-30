namespace Core.Contract.DTOs.Role;

public sealed record CreateFeaturePermissionDto(
      int Id,
      bool IsEnabled,
      bool IsCreated,
      bool IsUpdated,
      bool IsDeleted
);