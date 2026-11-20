using System.ComponentModel.DataAnnotations;
using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class TriggerSource 
{
      [Key]
      public int id { get; set; }
      public Guid guid {get; set;}
      public string name {get; set;} = string.Empty;
      public short value {get; set; }
      public EntityType entity_Type {get; set; }
      public ICollection<TriggerType> types {get; set; } = default!;
      public ICollection<Trigger> triggers {get; set; } = default!;
      public TriggerSource()
      {
      }
}