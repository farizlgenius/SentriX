using SharedKernel.Domain;

namespace Input.Infrastructure.Persistences.Entities;

public sealed class InputGroups : BaseEntity
{
      public string name {get; set;} = string.Empty;
      public string mac {get; set;} = string.Empty;
      public short device_component_id {get; set;}
      public string metadata {get; set;} = string.Empty;
      public string type {get; set;}  = string.Empty;

      public InputGroups()
      {
      }

      public InputGroups(Domain.Entities.InputGroups domain) : base(domain.ComponentId, domain.LocationId, domain.IsActive)
      {
            this.name = domain.Name;
            this.mac = domain.Mac;
            this.device_component_id = domain.DeviceComponentId;
            this.metadata = domain.Metadata;
            this.type = domain.Type;
            this.created_at = DateTime.UtcNow;
            this.updated_at = DateTime.UtcNow;
      }

      public void Update(Domain.Entities.InputGroups domain)
      {
            this.name = domain.Name;
            this.metadata = domain.Metadata;
            this.updated_at = DateTime.UtcNow;
      }


}