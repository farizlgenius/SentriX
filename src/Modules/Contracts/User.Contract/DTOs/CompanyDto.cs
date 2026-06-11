using SharedKernel.Domain;

namespace User.Contract.DTOs;

public sealed record CompanyDto(
      int Id,
      string Name,
      string Address,
      string Description,
      int LocationId,
      bool IsActive
) : BaseDto(
      0,
      LocationId,
      string.Empty,
      IsActive
);