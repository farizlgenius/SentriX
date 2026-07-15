using SharedKernel.Helpers;

namespace SharedKernel.Domain;

public class BaseDomainEntity
{
      public Guid Guid { get; set; }
      public short ComponentId { get; set; }
      public int LocationId { get; set; }
      public bool IsActive {get; set;}      
      public bool IsDefault {get; set;}

      public BaseDomainEntity(Guid Guid,short componentId,int locationId,bool IsActive,bool IsDefault)
      {
            ValidationHelper.ValidateNotMinus(locationId,nameof(LocationId));
            this.Guid = Guid;
            this.ComponentId = componentId;
            this.LocationId = locationId;
            this.IsActive = IsActive;
            this.IsDefault = IsDefault;
      }
}