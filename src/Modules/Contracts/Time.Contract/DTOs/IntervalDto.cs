using SharedKernel.Domain;

namespace Time.Contract.DTOs;

public sealed record IntervalDto(
       int Id,
      short ComponentId,
        DaysInWeekDto Days,
        string DaysDetail,
        string Start,
        string End,
        int LocationId,
        bool IsActive
) : BaseDto(ComponentId, LocationId, string.Empty, IsActive);