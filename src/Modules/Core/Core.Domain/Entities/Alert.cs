using SharedKernel.Enums;

namespace Core.Domain.Entities;

public sealed class Alert : BaseDomain
{
      public string Name { get; set; } = string.Empty;
      public string Advice {get; set; } = string.Empty;
      public int LocationId {get; set; } 
      public int AlertLevelId {get; set;}
      public List<int> AlertBaseEventIds {get; set;} = default!;
      public Alert(
            string name,
            string advice,
            int locationId,
            int alertLevel,
            List<int> baseEvent
      ) : base(Guid.NewGuid())
      {
            Name = name;
            Advice = advice;
            LocationId = locationId;
            AlertLevelId = alertLevel;
            AlertBaseEventIds = baseEvent;
      }
}

