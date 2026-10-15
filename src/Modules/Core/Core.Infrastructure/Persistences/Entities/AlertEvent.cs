using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class AlertEvent : BaseEntity
{
      public DateTime timestamp {get; set; }
      public string name {get; set; } = string.Empty;
      public EntityType entity {get; set; }
      public SharedKernel.Enums.BaseEvent base_event {get; set; }
      public bool acknowledge {get; set; }
      public bool postpone {get; set;}
      public string comment {get; set; } = string.Empty;
      public string alert_level {get; set; } =string.Empty;
      public string level_color {get; set ;} = string.Empty;
      // Relation
      public int? location_id {get; set; }
      public Location? location {get ;set;}
      public AlertEvent(){}
      public AlertEvent(Domain.Entities.AlertEvent d) : base(d.Guid)
      {
            timestamp = d.Timestamp;
            name = d.Name;
            entity = d.Entity;
            base_event = d.BaseEvent;
            acknowledge = d.Acknowledge;
            postpone = d.Postpone;
            comment = d.Comment;
            alert_level = d.AlertLevel;
            level_color = d.LevelColor;
            if(d.LocationId != 0)
            {
                  location_id = d.LocationId;
            }
            
      }

}