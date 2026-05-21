using SharedKernel.Domain;

namespace Time.Contract.DTOs;

public sealed record TimezoneDto(
        int Id,
        short ComponentId,
        string Name,
        short Mode,
        string Active,
        string Deactive,
        List<IntervalDto> Intervals,
        int LocationId,
        bool IsActive) : BaseDto(ComponentId,LocationId,string.Empty,IsActive);