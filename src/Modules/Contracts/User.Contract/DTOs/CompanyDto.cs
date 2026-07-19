using SharedKernel.Domain;

namespace User.Contract.DTOs;

public sealed record CompanyDto(
      Guid Guid = default,
      string Name = "",
      string Address = "",
      string Description = "",
      int LocationId = 0,
      bool IsActive = true,
      bool IsDefault = false
) : BaseDtoEntity(
      Guid,
      LocationId,
      string.Empty,
      IsActive,
      IsDefault
);