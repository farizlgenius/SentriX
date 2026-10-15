using System.ComponentModel.DataAnnotations;
using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class BaseEvent : BaseEntity
{
      public EntityType entity { get; set; }
      public SharedKernel.Enums.BaseEvent event_detail { get; set; }

      public string detail {get; set; } = string.Empty;
      public ICollection<AlertBaseEvent> alert_base_events {get; set ;} = default!;

      public BaseEvent (){}
}