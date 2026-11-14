using System.ComponentModel.DataAnnotations;
using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class TriggerVariableMapTrigger
{
      [Key]
      public int id {get; set; }
      public SharedKernel.Enums.TriggerVariableIndex index {get; set; }
      public int trigger_variable_id {get; set; }
      public TriggerVariable trigger_variable {get; set; } = default!;
      public int trigger_id {get; set; }
      public Trigger trigger {get; set; } = default!;
      public TriggerVariableMapTrigger(){}
      public TriggerVariableMapTrigger(
            TriggerVariableIndex Index,
            int triggerVariableId
      )
      {
            index = Index;
            trigger_variable_id = triggerVariableId;
      }

}