using SharedKernel.Domain;

namespace Time.Contract.DTOs;

public sealed record TimeZoneDto(
        Guid Guid=default,
        short ComponentId=0,
        string Name="",
        short Mode=0,
        string Active="",
        string Deactive="",
        List<IntervalDto> Intervals=default!,
        int LocationId=0,
        bool IsActive=true,
        bool IsDefault=false) : BaseDtoEntity(Guid,ComponentId,LocationId,string.Empty,IsActive,IsDefault);