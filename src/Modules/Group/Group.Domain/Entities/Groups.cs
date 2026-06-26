using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace Group.Domain.Entities;

public sealed class Groups : BaseDomain
{
      public string Name { get; private set; } = string.Empty;
      public List<GroupDoor> GroupDoors {get; private set;} = new List<GroupDoor>();

      public Groups(int id, short componentId,string name,List<(string Mac,string Type,List<(short DoorComponentId,short TimezoneComponentId)> DoorDetail)> doorGroup,int locationId, bool IsActive) : base(id, componentId, locationId, IsActive)
      {
            ValidationHelper.IsValidName(name);
            this.Name = name;
            this.GroupDoors = doorGroup.Select(x => new GroupDoor(
                  x.Mac,
                  x.Type,
                  x.DoorDetail.Select(s => (s.DoorComponentId,s.TimezoneComponentId)).ToList()
                  )).ToList();

      }
}