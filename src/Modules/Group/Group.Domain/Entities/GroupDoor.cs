using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace Group.Domain.Entities;

public sealed class GroupDoor 
{
      public string Mac { get; private set; } = string.Empty;
      public string Type {get; private set;} = string.Empty;
      public List<GroupDoorDetail> DoorDetails {get; private set;} = new List<GroupDoorDetail>();

      public GroupDoor(string mac,string Type,List<(short DoorComponentId,short TimezoneComponentId)> Doors) 
      {
            ValidationHelper.IsNullOrEmpty(mac,nameof(Mac));
            ValidationHelper.ValidateDeviceType(Type);
            this.Mac = mac;
            this.DoorDetails = Doors.Select(x => new GroupDoorDetail(
                  x.DoorComponentId,
                  x.TimezoneComponentId
            )).ToList();
            this.Type = Type;
      }
}