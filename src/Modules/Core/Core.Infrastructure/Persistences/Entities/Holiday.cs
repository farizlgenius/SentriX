namespace Core.Infrastructure.Persistences.Entities;

public sealed class Holiday : BaseEntity
{
  public string name { get; set; } = string.Empty;
  public DateTime start { get; set; }
  public DateTime end { get; set; }
  public int location_id { get; set; }
  public Location location { get; set; } = default!;
  public Holiday() { }
  public Holiday(Domain.Entities.Holiday d) : base(d.Guid)
  {
    name = d.Name;
    start = d.Start;
    end = d.End;
    location_id = d.LocationId;
  }
}