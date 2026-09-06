namespace Core.Domain.Entities;

public sealed class DayInWeek
{
  public bool Sunday { get; private set; }
  public bool Monday { get; private set; }
  public bool Tuesday { get; private set; }
  public bool Wednesday { get; private set; }
  public bool Thursday { get; private set; }
  public bool Friday { get; private set; }
  public bool Saturday { get; private set; }

  public DayInWeek(
    bool sun,
    bool mon,
    bool tue,
    bool wed,
    bool thu,
    bool fri,
    bool sat
  )
  {
    Sunday = sun;
    Monday = mon;
    Tuesday = tue;
    Wednesday = wed;
    Thursday = thu;
    Friday = fri;
    Saturday = sat;
  }
}