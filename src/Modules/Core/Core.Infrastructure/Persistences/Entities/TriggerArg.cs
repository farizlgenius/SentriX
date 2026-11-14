namespace Core.Infrastructure.Persistences.Entities;

public sealed class TriggerArg : BaseEntity
{
      public int trigger_code_id {get; set; }
      public TriggerCode trigger_code { get; set; } = default!;
      public short value {get; set; }
      public ICollection<TriggerArgMapTrigger> trigger_arg_map_trigger {get; set; } = default!;
      public TriggerArg(){}
}