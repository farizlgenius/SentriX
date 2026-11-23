using Core.Domain.Entities;
using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Action : BaseEntity
{
      public ActionType type { get; set; }
      public string? metadata {get; set;} 
      
      // Relate to location and device
      public int device_id {get; set;}
      public Device device {get; set;} = default!;
      public ICollection<ActionTarget> action_target {get; set; } = default!;
      public short sequence {get; set; }
      public int procedure_id {get; set; }
      public Procedure procedure {get; set; } = default!;

      public Action(){}
      public Action(Domain.Entities.Action d) : base(d.Guid,d.ExternalId,d.AltrExternalId)
      {
            name = d.Name;
            type = d.Type;
            metadata = d.Metadata;
            device_id = d.DeviceId;
            location_id = d.LocationId;
            sequence = d.Sequence;
            action_target = [.. d.ActionTarget.Select(x => new Persistences.Entities.ActionTarget(x))];
      }
}