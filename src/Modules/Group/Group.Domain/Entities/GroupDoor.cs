using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace Group.Domain.Entities;

public sealed class GroupDoor 
{
      public string Mac { get; private set; } = string.Empty;
      public short DeviceComponentId {get; private set;}
      public string Type {get; private set;} = string.Empty;
      public List<GroupDoorDetail> DoorDetails {get; private set;} = new List<GroupDoorDetail>();

      public GroupDoor(string mac,short deviceComponentId,string Type,List<(short DoorComponentId,short TimezoneComponentId)> Doors) 
      {
            ValidationHelper.IsNullOrEmpty(mac,nameof(Mac));
            ValidationHelper.ValidateNotMinus(deviceComponentId,nameof(DeviceComponentId));
            ValidationHelper.ValidateDeviceType(Type);
            this.Mac = mac;
            this.DeviceComponentId = deviceComponentId;
            this.DoorDetails = Doors.Select(x => new GroupDoorDetail(
                  x.DoorComponentId,
                  x.TimezoneComponentId
            )).ToList();
      }
}