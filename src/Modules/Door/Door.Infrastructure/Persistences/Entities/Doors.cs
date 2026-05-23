using SharedKernel.Domain;

namespace Door.Infrastructure.Persistences.Entities;

public sealed class Doors : BaseEntity
{
      public string name { get; set; } = string.Empty;
      public string mac {get; set;} = string.Empty;
      public short device_component_id {get; set;}
      public short second_component_id {get; set;}
      public string door_type {get; set;} = string.Empty;
      public string type {get; set;} = string.Empty;
      public string metadata {get; set;} = string.Empty;
      public Doors()
      {
      }

      public Doors(Domain.Entities.Doors domain) : base(domain.ComponentId,domain.LocationId,domain.IsActive)
      {
            this.name = domain.Name;
            this.mac = domain.Mac;
            this.device_component_id = domain.DeviceComponentId;
            this.second_component_id = domain.SecondComponentId;
            this.door_type = domain.DoorType;
            this.type = domain.Type;
            this.metadata = domain.Metadata;
            this.updated_at = DateTime.UtcNow;
            this.created_at = DateTime.UtcNow;
      }


      public void Update(Domain.Entities.Doors domain)
      {
            this.name = domain.Name;
            this.second_component_id = domain.SecondComponentId;
            this.door_type = domain.DoorType;
            this.type = domain.Type;
            this.metadata = domain.Metadata;
            this.updated_at = DateTime.UtcNow;
      }
}