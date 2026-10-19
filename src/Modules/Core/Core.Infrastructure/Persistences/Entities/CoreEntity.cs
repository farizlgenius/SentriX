using System.ComponentModel.DataAnnotations;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class CoreEntity 
{
      [Key]
      public int id {get; set; }
      public Guid guid {get; set ;}
      public SharedKernel.Enums.EntityType entity {get; set; }
      public ICollection<CoreEvent> events {get; set; } = default!;
      public CoreEntity(){}

}