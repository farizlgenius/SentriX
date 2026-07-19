using SharedKernel.Domain;

namespace User.Contract.DTOs;

public sealed record CreatePositionDto(
      string Name,
      string Description,
      Guid DepartmentGuid,
      int LocationId,
      bool IsActive,
      bool IsDefault
) : BaseDtoEntity(
      Guid.Empty,
      LocationId,
      string.Empty,
      IsActive,
      IsDefault
);