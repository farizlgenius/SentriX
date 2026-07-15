using SharedKernel.Domain;

namespace Time.Contract.DTOs;

public sealed record DaysInWeekDto(
        Guid Guid=default,
        bool Sunday=false,
        bool Monday=false,
        bool Tuesday=false,
        bool Wednesday=false,
        bool Thursday=false,
        bool Friday=false,
        bool Saturday=false
        );