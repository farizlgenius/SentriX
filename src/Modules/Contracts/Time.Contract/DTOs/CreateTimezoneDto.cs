using SharedKernel.Domain;

namespace Time.Contract.DTOs;

public sealed record CreateTimezoneDto(
      int Id,
        short ComponentId,
        string Name,
        short Mode,
        string Type,
        string Active,
        string Deactive,
        List<IntervalDto> Intervals,
        int LocationId,
        bool IsActive
) : BaseDto(ComponentId,LocationId,Type,IsActive);