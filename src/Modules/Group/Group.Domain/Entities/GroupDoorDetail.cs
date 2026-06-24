using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace Group.Domain.Entities;

public sealed class GroupDoorDetail 
{
      public short DoorComponentId { get; private set; }
      public short TimezoneComponentId {get; private set;}

      public GroupDoorDetail(short doorComponentId,short timezoneComponentId) 
      {
            ValidationHelper.ValidateNotMinus(doorComponentId,nameof(DoorComponentId));
            ValidationHelper.ValidateNotMinus(timezoneComponentId,nameof(TimezoneComponentId));
            this.DoorComponentId = doorComponentId;
            this.TimezoneComponentId = timezoneComponentId;

      }
}