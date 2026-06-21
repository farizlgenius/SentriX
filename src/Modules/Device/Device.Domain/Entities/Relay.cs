using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace Device.Domain.Entities;

public sealed class Relay : BaseDomain
{
      public int RelayNumber { get; private set; }
      public int ModuleId { get; private set; }

      public Relay(int id,short relayNumber,int moduleId,int locationId, bool IsActive) : base(id, 0, locationId, IsActive)
      {
            ValidationHelper.ValidateNotMinus(relayNumber,nameof(RelayNumber));
            ValidationHelper.ValidateNotMinus(moduleId,nameof(ModuleId));
            this.RelayNumber = relayNumber;
            this.ModuleId= moduleId;
      }
}