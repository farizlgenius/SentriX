using SharedKernel.Enums;

namespace Core.Domain.Entities;

public sealed class AlertEvent : BaseDomain
{
      public DateTime Timestamp { get; private set; }
       public string Name {get; private set; } = string.Empty;
      public EntityType Entity {get; private set; }
      public SharedKernel.Enums.BaseEvent BaseEvent {get; private set; }
      public bool Acknowledge {get; private set; }
      public bool Postpone {get; private set;}
      public string Comment {get; private set; } = string.Empty;
      public string AlertLevel {get; private set; } =string.Empty;
      public string LevelColor {get; set ;} = string.Empty;
      // Relation
      public int LocationId {get; private set; }
      public AlertEvent(
            DateTime time,
            string name,
            EntityType type,
            BaseEvent baseEvent,
            bool ack,
            bool post,
            string comment,
            string alert,
            string color
      ) : base(Guid.NewGuid())
      {
            Timestamp = time;
            Name = name;
            Entity = type;
            BaseEvent = baseEvent;
            Acknowledge = ack;
            Postpone = post;
            Comment = comment;
            AlertLevel = alert;
            LevelColor = color;
      }
}