using SharedKernel.Domain;

namespace Time.Contract.DTOs;

public sealed record CreateTimezoneDto(
        string Name,
        short Mode,
        string Type,
        string Active,
        string Deactive,
        List<IntervalDto> Intervals,
        int LocationId
) : BaseDtoEntity(default,0,LocationId,Type,true,false);