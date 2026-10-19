using SharedKernel.Enums;

namespace Core.Domain.Entities;

public sealed class AlertEvent : BaseDomain
{
      public DateTime Timestamp { get; private set; }
       public string Name {get; private set; } = string.Empty;
      public EntityType Entity {get; private set; }
      public SharedKernel.Enums.CoreEvent BaseEvent {get; private set; }
      public AlertAction AlertAction {get; private set; } = AlertAction.Pending;
      public string Comment {get; private set; } = string.Empty;
      public string AlertLevel {get; private set; } =string.Empty;
      public string LevelColor {get; set ;} = string.Empty;
      public string Username {get; private set;} = string.Empty;
      // Relation
      public int LocationId {get; private set; }
      public AlertEvent(
            DateTime time,
            string name,
            EntityType type,
            CoreEvent baseEvent,
            AlertAction alertAction,
            string comment,
            string alert,
            string color,
            string username
      ) : base(Guid.NewGuid())
      {
            Timestamp = time;
            Name = name;
            Entity = type;
            BaseEvent = baseEvent;
            AlertAction = alertAction;
            AlertLevel = alert;
            LevelColor = color;
            if (!string.IsNullOrWhiteSpace(username))
            {
                  Username = username;
            }
            if (!string.IsNullOrWhiteSpace(comment))
            {
                  Comment = comment;
            }
      }
}