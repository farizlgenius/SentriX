namespace Core.Contract.DTOs.Time;

public sealed record IntervalDto(
  DayInWeekDto Days,
  string Start,
  string End
);