using SharedKernel.Domain;

namespace Time.Contract.DTOs;

public sealed record IntervalDto(
        Guid Guid=default,
        DaysInWeekDto Days=default,
        string Start="",
        string End=""
);      