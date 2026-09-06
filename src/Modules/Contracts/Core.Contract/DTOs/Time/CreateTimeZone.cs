namespace Core.Contract.DTOs.Time;

public sealed record CreateTimeZoneDto(
  string Name,
  List<Guid> IntervalGuids,
  Guid LocationGuid
);