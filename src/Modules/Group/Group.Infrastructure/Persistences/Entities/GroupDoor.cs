using System.ComponentModel.DataAnnotations;
using SharedKernel.Domain;

namespace Group.Infrastructure.Persistences.Entities;

public sealed class GroupDoor
{
      [Key]
      public int id { get; set; }
      public string mac { get; private set; } = string.Empty;
      public string type { get; private set; } = string.Empty;
      public short device_component_id {get; set;}
      public short door_component_id { get; set; }
      public short timezone_component_id { get; set; }

      public Guid group_guid { get; set; }
      public Groups groups { get; set; } = default!;

      public GroupDoor()
      {

      }

      public GroupDoor(Domain.Entities.GroupDoor domain)
      {
            this.mac = domain.Mac;
            this.type = domain.Type;
            this.device_component_id = domain.DeviceComponentId;
            this.door_component_id = domain.DoorComponentId;
            this.timezone_component_id = domain.TimeZoneComponentId;
      }

      public void Update(Domain.Entities.GroupDoor domain)
      {
            this.mac = domain.Mac;
            this.type = domain.Type;
              this.device_component_id = domain.DeviceComponentId;
            this.door_component_id = domain.DoorComponentId;
            this.timezone_component_id = domain.TimeZoneComponentId;
      }
}