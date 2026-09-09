namespace Core.Infrastructure.Persistences.Entities;

public sealed class Turnstile : BaseEntity
{
      public string name { get; set; } = string.Empty;
      public ICollection<Lane> lanes { get; set; } = default!;
      // Relation
      public int location_id { get; set; }
      public Location location { get; set; } = default!;
      public Turnstile() { }
      public Turnstile(Domain.Entities.Turnstile d) : base(d.Guid)
      {
            name = d.Name;
            lanes = d.Lanes.Select(x => new Lane(x)).ToList();
            location_id = d.LocationId;
      }
}