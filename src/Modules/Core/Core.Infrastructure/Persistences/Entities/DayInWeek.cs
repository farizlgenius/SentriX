using System.ComponentModel.DataAnnotations;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class DayInWeek
{
  [Key]
  public int id { get; set; }
  public bool sunday { get; set; }
  public bool monday { get; set; }
  public bool tuesday { get; set; }
  public bool wednesday { get; set; }
  public bool thursday { get; set; }
  public bool friday { get; set; }
  public bool saturday { get; set; }
  public int interval_id { get; set; }
  public Interval interval { get; set; } = default!;
  public DayInWeek() { }

  public DayInWeek(
    Domain.Entities.DayInWeek d
  )
  {
    sunday = d.Sunday;
    monday = d.Monday;
    tuesday = d.Tuesday;
    wednesday = d.Wednesday;
    thursday = d.Thursday;
    friday = d.Friday;
    saturday = d.Saturday;
  }

}