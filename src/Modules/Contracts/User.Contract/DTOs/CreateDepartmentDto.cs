using SharedKernel.Domain;

namespace User.Contract.DTOs;

public sealed record CreateDepartmentDto(
      string Name,
      string Description,
      Guid CompanyGuid,
      int LocationId,
      bool IsActive,
      bool IsDefault
) : BaseDtoEntity(
      Guid.Empty,
      0,
      LocationId,
      string.Empty,
      IsActive,
      IsDefault
);