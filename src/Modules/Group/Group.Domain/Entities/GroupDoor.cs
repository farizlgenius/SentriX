using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace Group.Domain.Entities;

public sealed class GroupDoor 
{
      public string Mac { get; private set; } = string.Empty;
      public string Type {get; private set;} = string.Empty;
      public short DeviceComponentId {get; private set;}
      public short DoorComponentId {get; private set;}
      public short TimeZoneComponentId {get; private set;}

      public GroupDoor(string mac,string Type,short DeviceComponentId,short DoorComponentId,short TimeZoneComponentId) 
      {
            ValidationHelper.IsNullOrEmpty(mac,nameof(Mac));
            ValidationHelper.ValidateDeviceType(Type);
            this.Mac = mac;
            this.Type = Type;
            this.DeviceComponentId = DeviceComponentId;
            this.DoorComponentId = DoorComponentId;
            this.TimeZoneComponentId = TimeZoneComponentId;
      }
}