namespace Core.Contract.DTOs.Time;

public sealed record DayInWeekDto(
  bool Sunday = false,
  bool Monday = false,
  bool Tuesday = false,
  bool Wednesday = false,
  bool Thursday = false,
  bool Friday = false,
  bool Saturday = false
);