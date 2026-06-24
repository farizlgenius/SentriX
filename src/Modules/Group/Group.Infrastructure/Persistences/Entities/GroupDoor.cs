using SharedKernel.Domain;

namespace Group.Infrastructure.Persistences.Entities;

public sealed class GroupDoor 
{
      public string mac { get; private set; } = string.Empty;
      public short device_component_id {get; private set;}
      public string type {get; private set;} = string.Empty;
      public ICollection<GroupDoorDetail> group_door_detail {get; set;} = default!;

      public int group_id {get; set;}
      public Groups groups {get; set;} = default!;
      
      public GroupDoor()
      {
            
      }

      public GroupDoor(Domain.Entities.GroupDoor domain)
      {
            this.mac = domain.Mac;
            this.device_component_id = domain.DeviceComponentId;
            this.type = domain.Type;
            this.group_door_detail = domain.DoorDetails.Select(x => new GroupDoorDetail(x)).ToList();
      }

      public void Update(Domain.Entities.GroupDoor domain)
      {
            this.mac = domain.Mac;
            this.device_component_id = domain.DeviceComponentId;
            this.type = domain.Type;
      }
}