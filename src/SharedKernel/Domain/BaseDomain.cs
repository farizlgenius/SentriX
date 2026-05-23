using SharedKernel.Helpers;

namespace SharedKernel.Domain;

public class BaseDomain
{
      public int Id { get; set; }
      public short ComponentId { get; set; }
      public int LocationId { get; set; }
      public bool IsActive {get; set;}
      

      public BaseDomain(int id,short componentId,int locationId,bool IsActive)
      {
            ValidationHelper.ValidateNotMinus(locationId,nameof(LocationId));
            this.Id = id;
            this.ComponentId = componentId;
            this.LocationId = locationId;
            this.IsActive = IsActive;
      }
}