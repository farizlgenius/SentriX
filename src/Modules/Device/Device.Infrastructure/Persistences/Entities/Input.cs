using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace Device.Infrastructure.Persistences.Entities;

public sealed class Input : BaseEntity
{
      public int input_number { get; private set; }
      public int module_id { get; private set; }
      public Module module { get; private set; } = default!;
      public Input(){}
      public Input(Domain.Entities.Input domain) : base(
            0,
            domain.LocationId,
            domain.IsActive,
            false
      )
      {
            this.input_number = domain.InputNumber;
            this.module_id = domain.ModuleId;
      
      }
}