namespace Core.Contract.DTOs.Time;

public sealed record CreateHolidayDto(
  string Name,
  DateTime Start,
  DateTime End,
  bool IsActive,
  bool IsDefault,
  Guid LocationGuid
);