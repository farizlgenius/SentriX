using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace Device.Domain.Entities;

public sealed class Input 
{
      public Guid Guid { get; set; }
      public int InputNumber { get; private set; }
      public Guid ModuleGuid { get; private set; }
      public int LocationId { get; set; }

      public Input(Guid guid,short inputNumber,Guid moduleGuid,int locationId) 
      {
            ValidationHelper.ValidateGuid(guid,nameof(Guid));
            ValidationHelper.ValidateNotMinus(inputNumber,nameof(InputNumber));
            ValidationHelper.ValidateGuid(moduleGuid,nameof(ModuleGuid));
            this.Guid = guid;
            this.InputNumber = inputNumber;
            this.ModuleGuid= moduleGuid;
            this.LocationId = locationId;
      }
}