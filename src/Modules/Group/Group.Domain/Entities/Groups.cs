using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace Group.Domain.Entities;

public sealed class Groups : BaseDomainEntity
{
      public string Name { get; private set; } = string.Empty;
      public List<GroupDoor> GroupDoors {get; private set;} = new List<GroupDoor>();

      public Groups(
            Guid guid, 
            string name,
            List<GroupDoor> doorGroup,
            int locationId, 
            bool IsActive,
            bool IsDefault
            ) : base(guid, locationId, IsActive,IsDefault)
      {
            ValidationHelper.IsValidName(name);
            this.Name = name;
            this.GroupDoors = doorGroup.Select(x => new GroupDoor(
                  x.Mac,
                  x.Type,
                  x.DeviceComponentId,
                  x.DoorComponentId,
                  x.TimeZoneComponentId
            )).ToList();

      }
}