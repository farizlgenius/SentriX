using SharedKernel.Interfaces;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class MonitorGroup : BaseEntity, IAuditableEntity
{
      public ICollection<MonitorGroupComponent> components {get; set; } = default!;
      // Relation
      public int device_id {get; set; }
      public Device device {get; set; } = default!;
      public MonitorGroup(){}
      public MonitorGroup(
            Domain.Entities.MonitorGroup d
      ) : base(d.Guid,d.ExternalId)
      {
            name = d.Name;
            this.components = [.. d.Components.Select(x => new Persistences.Entities.MonitorGroupComponent(x.Type,x.ComponentId))];
            if(d.DeviceId != 0)
                  device_id = d.DeviceId;
      }
}