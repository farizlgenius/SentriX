namespace Core.Infrastructure.Persistences.Entities;

public sealed class TriggerVariable : BaseEntity
{
      // Relation 
      public int trigger_id {get; set; }
      public Trigger trigger {get; set; } = default!;
      public int device_id {get; set; }
      public Device device {get; set; } = default!;
      public ICollection<TriggerVariableMapTrigger> trigger_variable_map_trigger {get; set; } = default!;
      
      public TriggerVariable(){}
      public TriggerVariable(
            Domain.Entities.TriggerVariable d
      ) : base(d.Guid,d.ExternalId)
      {
            trigger_id = d.TriggerId;
            device_id = d.DeviceId;
      }
}