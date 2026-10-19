using System.ComponentModel.DataAnnotations;
using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class AlertComponent
{
      [Key]
      public int id { get; set; }
      public int alert_id { get; set; }
      public Alert alert { get; set; } = default!;
      // Component
      public int door_id { get; set; }
      public Door door { get; set; } = default!;
      public AlertComponent() { }
      // Door
      public AlertComponent(EntityType type, int componentId)
      {
            switch (type)
            {
                  case EntityType.DOOR:
                        door_id = componentId;
                        break;
                  default:
                        break;
            }
      }
}