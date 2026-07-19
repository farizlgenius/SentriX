using SharedKernel.Domain;

namespace User.Contract.DTOs;

public sealed record CreateCompanyDto(
      string Name,
      string Address,
      string Description,
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