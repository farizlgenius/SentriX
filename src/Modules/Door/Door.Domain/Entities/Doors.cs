using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace Door.Domain.Entities;

public sealed class Doors : BaseDomain
{
      public string Name { get; set; } = string.Empty;
      public string Mac {get; set;} = string.Empty;
      public short DeviceComponentId {get; set;}
      public short SecondComponentId {get; set;}
      public string DoorType {get; set;} = string.Empty;
      public string Metadata {get; set;} = string.Empty;
      public string Type {get; set;} = string.Empty;
      
      public Doors(
            int id, 
            short deviceComponentId,
            string mac,
            short componentId,
            short secondComponentId,
            string name,
            string doorType,
            string metadata,
            string type,
            int locationId, 
            bool IsActive) : base(id, componentId, locationId, IsActive)
      {
            ValidationHelper.IsValidName(name);
            ValidationHelper.IsNullOrEmpty(doorType,nameof(DoorType));
            ValidationHelper.IsNullOrEmpty(mac,nameof(Mac));
            ValidationHelper.IsNullOrEmpty(metadata,nameof(Metadata));
            ValidationHelper.IsNullOrEmpty(type,nameof(Type));
            this.Name = name;
            this.DeviceComponentId =deviceComponentId;
            this.SecondComponentId = secondComponentId;
            this.Mac = mac;
            this.DoorType = doorType;
            this.Metadata = metadata;
            this.Type = type;
            this.DoorType = doorType;
      }
}