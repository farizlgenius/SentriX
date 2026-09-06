namespace Core.Contract.DTOs.Time;

public sealed record UpdateTimeZoneDto(
  Guid Guid,
  string Name,
  List<Guid> IntervalGuids,
  Guid LocationGuid
);