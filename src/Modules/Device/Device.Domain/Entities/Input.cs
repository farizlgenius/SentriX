using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace Device.Domain.Entities;

public sealed class Input : BaseDomain
{
      public int InputNumber { get; private set; }
      public int ModuleId { get; private set; }

      public Input(int id,short inputNumber,int moduleId,int locationId, bool IsActive) : base(id, 0, locationId, IsActive)
      {
            ValidationHelper.ValidateNotMinus(inputNumber,nameof(InputNumber));
            ValidationHelper.ValidateNotMinus(moduleId,nameof(ModuleId));
            this.InputNumber = inputNumber;
            this.ModuleId= moduleId;
      }
}