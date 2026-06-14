using SharedKernel.Domain;

namespace User.Contract.DTOs;

public sealed record CreateDepartmentDto(
      string Name,
      string Description,
      int CompanyId,
      int LocationId,
      bool IsActive
) : BaseDto(
      0,
      LocationId,
      string.Empty,
      IsActive
);