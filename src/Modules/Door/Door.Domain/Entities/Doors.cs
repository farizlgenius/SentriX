using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace Door.Domain.Entities;

public sealed class Doors : BaseDomainEntity
{
      public string Name { get; private set; } = string.Empty;
      public string Mac {get; private set;} = string.Empty;
      public short DeviceComponentId {get; private set;}
      public short SecondComponentId {get; private set;}
      public string DoorType {get; private set;} = string.Empty;
      public string Metadata {get; private set;} = string.Empty;
      public string Type {get; private set;} = string.Empty;
      
      public Doors(
            Guid guid,
            short deviceComponentId,
            string mac,
            short componentId,
            short secondComponentId,
            string name,
            string doorType,
            string metadata,
            string type,
            int locationId, 
            bool IsActive,
            bool IsDefault) : base(guid, componentId, locationId, IsActive,IsDefault)
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