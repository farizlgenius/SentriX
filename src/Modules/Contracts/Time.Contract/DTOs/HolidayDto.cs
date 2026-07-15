using SharedKernel.Domain;

namespace Time.Contract.DTOs;

public sealed record HolidayDto(
       Guid Guid=default,
       short ComponentId=0,
        string Name="",
        DateTime? Start=null,
        DateTime? End=null,
        int LocationId=0,
        bool IsActive=false,
        bool IsDefault=false
) : BaseDtoEntity(Guid,ComponentId,LocationId,string.Empty,IsActive,IsDefault);