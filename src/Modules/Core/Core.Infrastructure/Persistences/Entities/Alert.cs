using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Alert : BaseEntity
{
      public string name {get; set; } = string.Empty;
      public string advice {get; set;} = string.Empty;
      public bool is_email {get; set; }
      // Relation
      public int alert_level_id {get; set; }
      public AlertLevel alert_level {get; set; }  = default!;
      public ICollection<AlertEventDetail> alert_event_details {get; set ;} = default!;
      public Alert(){} 
}