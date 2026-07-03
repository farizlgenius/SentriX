using SharedKernel.Domain;

namespace Device.Infrastructure.Persistences.Entities;

public sealed class Relay : BaseEntity
{
      public int relay_number { get; set; }
      public int module_id { get; set; }
      public Module module { get; set; } = default!;

      public Relay(){}

      public Relay(Domain.Entities.Relay domain) : base(
0,
domain.LocationId,
domain.IsActive,
false
)
      {
            this.relay_number = domain.RelayNumber;
            this.module_id = domain.ModuleId;

      }

}