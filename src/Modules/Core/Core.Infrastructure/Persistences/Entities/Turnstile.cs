using SharedKernel.Interfaces;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Turnstile : BaseEntity,IAuditableEntity
{
      public ICollection<Lane> lanes { get; set; } = default!;
      // Relation
      public Turnstile() { }
      public Turnstile(Domain.Entities.Turnstile d) : base(d.Guid)
      {
            name = d.Name;
            lanes = d.Lanes.Select(x => new Lane(x)).ToList();
            location_id = d.LocationId;
      }
}