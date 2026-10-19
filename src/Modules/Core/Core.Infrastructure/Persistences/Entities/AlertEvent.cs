using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class AlertEvent : BaseEntity
{
      public DateTime timestamp {get; set; }
      public string name {get; set; } = string.Empty;
      public EntityType entity {get; set; }
      public SharedKernel.Enums.CoreEvent base_event {get; set; }
      public AlertAction alert_action {get; set; } = AlertAction.Pending;
      public string? comment {get; set; } 
      public string alert_level {get; set; } =string.Empty;
      public string level_color {get; set ;} = string.Empty;
      public string? username {get; set; } 
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
            alert_action = d.AlertAction;
            alert_level = d.AlertLevel;
            level_color = d.LevelColor;
            if(d.LocationId != 0)
            {
                  location_id = d.LocationId;
            }
            if (!string.IsNullOrWhiteSpace(d.Username))
            {
                  username = d.Username;
            }
            if (!string.IsNullOrWhiteSpace(d.Comment))
            {
                  comment = d.Comment;
            }
            
      }

}