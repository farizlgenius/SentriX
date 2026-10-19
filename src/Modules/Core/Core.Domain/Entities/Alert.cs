using SharedKernel.Enums;

namespace Core.Domain.Entities;

public sealed class Alert : BaseDomain
{
      public string Name { get; private set; } = string.Empty;
      public string Advice {get; private set; } = string.Empty;
      public int LocationId {get; private set; } 
      public int AlertLevelId {get; private set;}
      public int CoreEventId {get; private set; }
      public List<int> ComponentIds {get; private set;} = default!;
      public EntityType EntityType {get; private set;}
      public Alert(
            string name,
            string advice,
            int locationId,
            int alertLevel,
            int coreEventId,
            EntityType type,
            List<int> componentId
      ) : base(Guid.NewGuid())
      {
            Name = name;
            Advice = advice;
            LocationId = locationId;
            AlertLevelId = alertLevel;
            CoreEventId = coreEventId;
            ComponentIds = componentId;
            EntityType = type;
      }
}

