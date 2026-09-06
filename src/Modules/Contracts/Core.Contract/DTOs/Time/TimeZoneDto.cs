namespace Core.Contract.DTOs.Time;

public sealed record TimeZoneDto(
  Guid Guid,
  string Name,
  List<IntervalDto> Intervals,
  Guid LocationGuid,
  bool IsActive,
  bool IsDefault
);