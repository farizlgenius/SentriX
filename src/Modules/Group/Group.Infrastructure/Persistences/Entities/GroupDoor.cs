using System.ComponentModel.DataAnnotations;
using SharedKernel.Domain;

namespace Group.Infrastructure.Persistences.Entities;

public sealed class GroupDoor 
{
      [Key]
      public int id {get; set;}
      public string mac { get; private set; } = string.Empty;
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
            this.type = domain.Type;
            this.group_door_detail = domain.DoorDetails.Select(x => new GroupDoorDetail(x)).ToList();
      }

      public void Update(Domain.Entities.GroupDoor domain)
      {
            this.mac = domain.Mac;
            this.type = domain.Type;
      }
}