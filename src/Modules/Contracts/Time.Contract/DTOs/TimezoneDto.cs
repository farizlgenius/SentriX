using SharedKernel.Domain;

namespace Time.Contract.DTOs;

public sealed record TimezoneDto(
        int Id=0,
        short ComponentId=0,
        string Name="",
        short Mode=0,
        string Active="",
        string Deactive="",
        List<IntervalDto> Intervals=default!,
        int LocationId=0,
        bool IsActive=false,
        bool IsDefault=false) : BaseDto(ComponentId,LocationId,string.Empty,IsActive);