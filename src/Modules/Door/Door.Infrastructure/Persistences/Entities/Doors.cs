using SharedKernel.Domain;

namespace Door.Infrastructure.Persistences.Entities;

public sealed class Doors : BaseDbEntity
{
      public string name { get; set; } = string.Empty;
      public string door_type {get; set;} = string.Empty;
      public string type {get; set;} = string.Empty;
      public string metadata {get; set;} = string.Empty;
      
      public Doors()
      {
      }

      public Doors(Domain.Entities.Doors domain) : base(domain.Guid,domain.LocationId,domain.IsActive,false)
      {
            this.name = domain.Name;
            this.door_type = domain.DoorType;
            this.type = domain.Type;
            this.metadata = domain.Metadata;
            this.updated_at = DateTime.UtcNow;
            this.created_at = DateTime.UtcNow;
      }


      public void Update(Domain.Entities.Doors domain)
      {
            this.name = domain.Name;
            this.door_type = domain.DoorType;
            this.type = domain.Type;
            this.metadata = domain.Metadata;
            this.updated_at = DateTime.UtcNow;
      }
}