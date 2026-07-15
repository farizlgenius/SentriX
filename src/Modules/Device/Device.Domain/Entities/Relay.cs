using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace Device.Domain.Entities;

public sealed class Relay 
{
      public Guid Guid { get; set; }
      public int RelayNumber { get; private set; }
      public Guid ModuleGuid { get; private set; }
      public int LocationId { get; set; }

      public Relay(Guid guid,short relayNumber,Guid moduleGuid,int locationId) 
      {
            ValidationHelper.ValidateNotMinus(relayNumber,nameof(RelayNumber));
            this.Guid = guid;
            this.RelayNumber = relayNumber;
            this.ModuleGuid= moduleGuid;
            this.LocationId = locationId;
      }
}