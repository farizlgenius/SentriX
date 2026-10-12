using Core.Domain.Entities;
using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Action : BaseEntity
{
      public string name {get; set; } = string.Empty;
      public ActionType type { get; set; }
      public string metadata {get; set;} = string.Empty;
      
      // Relate to location and device
      public int device_id {get; set;}
      public Device device {get; set;} = default!;
      public int location_id {get; set;}
      public Location location {get; set; } =default!;
      public ICollection<ProcedureAction> procedure_actions {get; set;} = default!;

      public Action(){}
      public Action(Actions d) : base(d.Guid)
      {
            name = d.Name;
            type = d.Type;
            metadata = d.Metadata;
            device_id = d.DeviceId;
            location_id = d.LocationId;
      }
}