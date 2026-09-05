namespace Core.Contract.DTOs.Time;

public sealed record DayInWeekDto(
  bool Sunday,
  bool Monday,
  bool Tuesday,
  bool Wednesday,
  bool Thursday,
  bool Friday,
  bool Saturday
);