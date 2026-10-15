using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Alert : BaseEntity
{
      public string name {get; set; } = string.Empty;
      public string advice {get; set;} = string.Empty;
      // Relation
      public int location_id {get; set;}
      public Location location {get; set; } = default!;
      public int alert_level_id {get; set; }
      public AlertLevel alert_level {get; set; }  = default!;
      public ICollection<AlertBaseEvent> alert_base_events {get; set ;} = default!;
      public Alert(){} 
      public Alert(Domain.Entities.Alert d) : base(d.Guid)
      {
            name = d.Name;
            advice = d.Advice;
            location_id = d.LocationId;
            alert_level_id = d.AlertLevelId;
            alert_base_events = d.AlertBaseEventIds.Select(x => new AlertBaseEvent(x)).ToList();
      }
}