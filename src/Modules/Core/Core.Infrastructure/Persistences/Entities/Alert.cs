using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Alert : BaseEntity
{
      public string advice {get; set;} = string.Empty;
      // Relation
      public int alert_level_id {get; set; }
      public AlertLevel alert_level {get; set; }  = default!;
      public int core_event_id {get; set;}
      public Persistences.Entities.CoreEvent core_event {get; set;} = default!;
      // Component 
      public ICollection<AlertComponent> alert_components {get; set; } = default!; 
      public Alert(){} 
      public Alert(Domain.Entities.Alert d) : base(d.Guid)
      {
            name = d.Name;
            advice = d.Advice;
            location_id = d.LocationId;
            alert_level_id = d.AlertLevelId;
            core_event_id = d.CoreEventId;
      }
}