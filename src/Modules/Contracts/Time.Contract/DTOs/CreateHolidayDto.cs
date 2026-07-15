using SharedKernel.Domain;

namespace Time.Contract.DTOs;

public sealed record CreateHolidayDto(
        string Name,
        DateTime Start,
        DateTime End,
        int LocationId,
        bool IsActive,
        bool IsDefault
) : BaseDtoEntity(Guid.Empty,0,LocationId, string.Empty, IsActive,IsDefault);