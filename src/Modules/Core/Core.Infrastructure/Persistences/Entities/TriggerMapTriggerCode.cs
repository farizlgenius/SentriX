namespace Core.Infrastructure.Persistences.Entities;

public sealed class TriggerMapTriggerCode
{
      public int trigger_id {get; set; }
      public Trigger trigger {get; set; } = default!;
      public int trigger_code_id {get; set; }
      public TriggerCode trigger_code {get; set; } = default!;
      public TriggerMapTriggerCode(){}
      public TriggerMapTriggerCode(int triggerCode)
      {
            trigger_code_id = triggerCode;
      }
}