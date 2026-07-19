using SharedKernel.Domain;

namespace Time.Contract.DTOs;

public sealed record IntervalDto(
        Guid Guid=default,
        short ComponentId=0,
        DaysInWeekDto Days=default,
        string Start="",
        string End=""
);      