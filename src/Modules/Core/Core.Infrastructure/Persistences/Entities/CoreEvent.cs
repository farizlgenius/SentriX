using System.ComponentModel.DataAnnotations;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class CoreEvent
{
      [Key]
      public int id {get; set; }
      public Guid guid {get; set ;}
      public SharedKernel.Enums.CoreEvent @event {get; set; }
      public int entity_id {get; set; }
      public CoreEntity entity {get; set; } = default!;
      // Releation 
      public ICollection<Alert> alerts {get; set; } = default!;

      public CoreEvent(){}

}