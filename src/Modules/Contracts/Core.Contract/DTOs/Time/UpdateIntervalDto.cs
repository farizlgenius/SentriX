namespace Core.Contract.DTOs.Time;

public sealed record UpdateIntervalDto(
  Guid Guid,
  DayInWeekDto Days,
  TimeOnly Start,
  TimeOnly End,
  Guid LocationGuid
);