using SharedKernel.Interfaces;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Holiday : BaseEntity,IAuditableEntity
{
  public DateTime start { get; set; }
  public DateTime end { get; set; }
  public Holiday() { }
  public Holiday(Domain.Entities.Holiday d) : base(d.Guid)
  {
    name = d.Name;
    start = d.Start;
    end = d.End;
    location_id = d.LocationId;
  }
}