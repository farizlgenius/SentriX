namespace Core.Contract.DTOs.Time;

public sealed record CreateIntervalDto(
  DayInWeekDto Days,
  TimeOnly Start,
  TimeOnly End,
  Guid LocationGuid
);