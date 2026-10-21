using Core.Domain.Entities;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Procedure : BaseEntity
{
      public int device_id {get; set; }
      public Device device {get; set; } = default!;
      public ICollection<ProcedureAction> procedure_actions {get; set; }= default!;

      public Procedure(){}
      public Procedure(
            Domain.Entities.Procedure d
      ) : base(d.Guid)
      {
            name = d.Name;
            device_id = d.DeviceId;
            procedure_actions = d.ActionIds.Select(x => new ProcedureAction(x)).ToList();
      }
}