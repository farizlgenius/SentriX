using SharedKernel.Domain;

namespace User.Contract.DTOs;

public sealed record DepartmentDto(
      Guid Guid=default,
      string Name="",
      string Description="",
      Guid CompanyGuid=default,
      int LocationId=0,
      bool IsActive=true,
      bool IsDefault=false
) : BaseDtoEntity(
      Guid,
      LocationId,
      string.Empty,
      IsActive,
      IsDefault
);