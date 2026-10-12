using System.ComponentModel.DataAnnotations;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class TriggerSource 
{
      [Key]
      public int id { get; set; }
      public Guid guid {get; set;}
      public string name {get; set;} = string.Empty;
      public short value {get; set; }
      public ICollection<TriggerType> types {get; set; } = default!;
      public TriggerSource()
      {
      }
}