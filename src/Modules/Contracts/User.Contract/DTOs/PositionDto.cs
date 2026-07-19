using SharedKernel.Domain;

namespace User.Contract.DTOs;

public sealed record PositionDto(
       Guid Guid=default,
      string Name="",
      string Description="",
      Guid DepartmentGuid=default,
      int LocationId=0,
      bool IsActive=true,
      bool IsDefault=false
) : BaseDtoEntity(
      Guid,
      0,
      LocationId,
      string.Empty,
      IsActive,
      IsDefault
);