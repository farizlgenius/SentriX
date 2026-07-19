using SharedKernel.Domain;

namespace Time.Contract.DTOs;

public sealed record HolidayDto(
       Guid Guid=default,
        string Name="",
        DateTime? Start=null,
        DateTime? End=null,
        int LocationId=0,
        bool IsActive=false,
        bool IsDefault=false
) : BaseDtoEntity(Guid,LocationId,string.Empty,IsActive,IsDefault);