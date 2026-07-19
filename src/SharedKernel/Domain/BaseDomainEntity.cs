using SharedKernel.Helpers;

namespace SharedKernel.Domain;

public class BaseDomainEntity
{
      public Guid Guid { get; set; }
      public int LocationId { get; set; }
      public bool IsActive {get; set;}      
      public bool IsDefault {get; set;}

      public BaseDomainEntity(Guid Guid,int locationId,bool IsActive,bool IsDefault)
      {
            ValidationHelper.ValidateNotMinus(locationId,nameof(LocationId));
            this.Guid = Guid;
            this.LocationId = locationId;
            this.IsActive = IsActive;
            this.IsDefault = IsDefault;
      }
}