using SharedKernel.Domain;

namespace Time.Contract.DTOs;

public sealed record TimeZoneDto(
        Guid Guid=default,
        string Name="",
        List<IntervalDto> Intervals=default!,
        int LocationId=0,
        bool IsActive=true,
        bool IsDefault=false) : BaseDtoEntity(Guid,LocationId,string.Empty,IsActive,IsDefault);