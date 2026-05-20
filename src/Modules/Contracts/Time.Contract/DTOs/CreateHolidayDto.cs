using SharedKernel.Domain;

namespace Time.Contract.DTOs;

public sealed record CreateHolidayDto(
        string Name,
        short Year,
        short Month,
        short Day,
        string Metadata,
        int LocationId,
        bool IsActive
) : BaseDto(0, LocationId, string.Empty, IsActive);