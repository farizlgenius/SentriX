using System.ComponentModel.DataAnnotations;
using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class MonitorGroupComponent
{
      [Key]
      public int id {get; set; }
      public MonitorType type {get; set; }
      public int? input_id {get; set; }
      public Input? input {get; set; } = default!;
      public int? door_id {get; set; }
      public Door? door {get; set; }
      public int monitor_group_id {get; set; }
      public MonitorGroup monitor_group {get; set; } =default!;
      public MonitorGroupComponent(){}
      public MonitorGroupComponent(
            MonitorType type,
            int componentId
      )
      {
            switch (type)
            {
                  case MonitorType.MONITOR_POINT:
                  input_id = componentId;
                  break;
                  case MonitorType.HELD_OPEN:
                  case MonitorType.FORCED_OPEN:
                  door_id = componentId;
                  break;
                  default:
                  break;
            }
      }
}