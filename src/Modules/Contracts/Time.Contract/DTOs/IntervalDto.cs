using SharedKernel.Domain;

namespace Time.Contract.DTOs;

public sealed record IntervalDto(
        Guid Guid=default,
        int ComponentId=0,
        DaysInWeekDto Days=default,
        string DaysDetail="",
        string Start="",
        string End=""
);