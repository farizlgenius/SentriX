using SharedKernel.Domain;

namespace Time.Contract.DTOs;

public sealed record CreateTimezoneDto(
        string Name,
        List<IntervalDto> Intervals,
        int LocationId
) : BaseDtoEntity(default,LocationId,string.Empty,true,false);