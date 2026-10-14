using System.ComponentModel.DataAnnotations;
using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class EventDetail : BaseEntity
{
      public EntityType entity { get; set; }
      public SharedKernel.Enums.EventDetail event_detail { get; set; }

      public string detail {get; set; } = string.Empty;
      public ICollection<AlertEventDetail> alert_event_details {get; set ;} = default!;

      public EventDetail (){}
}