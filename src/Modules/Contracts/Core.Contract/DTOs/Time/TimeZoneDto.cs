namespace Core.Contract.DTOs.Time;

public sealed record TimeZoneDto(
  Guid Guid,
  string Name,
  List<IntervalDto> Intervals,
  string LocationGuid,
  bool IsActive,
  bool IsDefault
);