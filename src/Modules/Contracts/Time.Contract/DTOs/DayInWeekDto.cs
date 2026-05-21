using SharedKernel.Domain;

namespace Time.Contract.DTOs;

public sealed record DaysInWeekDto(
        int Id,
        short ComponentId,
        bool Sunday,
        bool Monday,
        bool Tuesday,
        bool Wednesday,
        bool Thursday,
        bool Friday,
        bool Saturday,
        int LocationId
        );