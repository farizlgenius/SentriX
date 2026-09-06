using System.ComponentModel.DataAnnotations;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class TimeZoneInterval
{
  [Key]
  public int id { get; set; }
  public int timezone_id { get; set; }
  public TimeZone timezone { get; set; } = default!;
  public int interval_id { get; set; }
  public Interval interval { get; set; } = default!;

  public TimeZoneInterval() { }
  public TimeZoneInterval(
    int timezoneId,
    int intervalId
  )
  {
    if (timezoneId == 0)
    {
      interval_id = intervalId;
    }
    else if (intervalId == 0)
    {
      timezone_id = timezoneId;
    }
    else
    {
      timezone_id = timezoneId;
      interval_id = intervalId;
    }
  }
}