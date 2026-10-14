using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class AlertEvent : BaseEntity
{
      public DateTime timestamp {get; set; }
      public string name {get; set; } = string.Empty;
      public EntityType entity {get; set; }
      public SharedKernel.Enums.EventDetail event_detail {get; set; }
      public string acknowledge {get; set; } = string.Empty;
      public string detail {get; set; } = string.Empty;
      public string alert_level {get; set; } =string.Empty;
      public string level_color {get; set ;} = string.Empty;
      public AlertEvent(){}
}