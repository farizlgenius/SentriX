using System.ComponentModel.DataAnnotations;
using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class TriggerArgMapTrigger
{
      [Key]
      public int id {get; set; }
      public int trigger_arg_id {get; set; }
      public TriggerArg trigger_arg {get; set; } = default!;
      public int trigger_id {get; set; }
      public Trigger trigger {get; set; } = default!;
      public TriggerArgMapTrigger(){}
      public TriggerArgMapTrigger(
            int triggerArgId
      )
      {
            trigger_arg_id = triggerArgId;
      }

}