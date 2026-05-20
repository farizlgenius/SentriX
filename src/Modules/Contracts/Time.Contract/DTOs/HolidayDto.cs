using SharedKernel.Domain;

namespace Time.Contract.DTOs;

public sealed record HolidayDto(
       int Id,
        short ComponentId,
        string Name,
        short Year,
        short Month,
        short Day,
        string Metadata,
        int LocationId,
        bool IsActive
) : BaseDto(ComponentId,LocationId,string.Empty,IsActive);