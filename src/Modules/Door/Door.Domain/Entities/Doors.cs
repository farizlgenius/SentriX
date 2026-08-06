using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace Door.Domain.Entities;

public sealed class Doors : BaseDomainEntity
{
      public string Name { get; private set; } = string.Empty;
      public string DoorType {get; private set;} = string.Empty;
      public string Metadata {get; private set;} = string.Empty;
      public string Type {get; private set;} = string.Empty;
      
      public Doors(
            Guid guid,
            string name,
            string doorType,
            string metadata,
            string type,
            int locationId, 
            bool IsActive,
            bool IsDefault) : base(guid, locationId, IsActive,IsDefault)
      {
            ValidationHelper.IsValidName(name);
            ValidationHelper.IsNullOrEmpty(doorType,nameof(DoorType));
            ValidationHelper.IsNullOrEmpty(metadata,nameof(Metadata));
            ValidationHelper.IsNullOrEmpty(type,nameof(Type));
            this.Name = name;
            this.DoorType = doorType;
            this.Metadata = metadata;
            this.Type = type;
            this.DoorType = doorType;
      }
}