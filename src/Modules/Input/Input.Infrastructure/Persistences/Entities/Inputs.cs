using SharedKernel.Domain;

namespace Input.Infrastructure.Persistences.Entities;

public sealed class Inputs : BaseEntity
{
      public string name {get; set;} = string.Empty;
      public string mac {get; set;} = string.Empty;
      public short device_component_id {get; set;}
      public short module_component_id {get; set;}
      public short input_no {get; set;}
      public string metadata {get; set;} = string.Empty;
      public string type {get; set;}  = string.Empty;

      public Inputs()
      {
      }

      public Inputs(Domain.Entities.Inputs domain) : base(domain.ComponentId, domain.LocationId, domain.IsActive)
      {
            this.name = domain.Name;
            this.mac = domain.Mac;
            this.device_component_id = domain.DeviceComponentId;
            this.module_component_id = domain.ModuleComponentId;
            this.input_no = domain.InputNo;
            this.metadata = domain.Metadata;
            this.type = domain.Type;
            this.created_at = DateTime.UtcNow;
            this.updated_at = DateTime.UtcNow;
      }

      public void Update(Domain.Entities.Inputs domain)
      {
            this.name = domain.Name;
            this.input_no = domain.InputNo;
            this.metadata = domain.Metadata;
            this.updated_at = DateTime.UtcNow;
      }


}