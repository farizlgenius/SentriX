using SharedKernel.Domain;

namespace Group.Infrastructure.Persistences.Entities;

public sealed class GroupDoorDetail 
{
      public short door_component_id {get; set;}
      public short timezone_component_id {get; set;}
      public int group_door_id {get; set;}
      public GroupDoor group_door {get; set;} = default!;
      
      public GroupDoorDetail()
      {
            
      }

      public GroupDoorDetail(Domain.Entities.GroupDoorDetail domain)
      {
            this.door_component_id = domain.DoorComponentId;
            this.timezone_component_id = domain.TimezoneComponentId;
      }

      public void Update(Domain.Entities.GroupDoorDetail domain)
      {
            this.door_component_id = domain.DoorComponentId;
            this.timezone_component_id = domain.TimezoneComponentId;
      }
}