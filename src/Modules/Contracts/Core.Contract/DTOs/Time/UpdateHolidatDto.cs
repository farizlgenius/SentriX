namespace Core.Contract.DTOs.Time;

public sealed record UpdateHolidayDto(
  Guid Guid,
  string Name,
  DateTime Start,
  DateTime End,
  bool IsActive,
  bool IsDefault,
  Guid LocationGuid
);