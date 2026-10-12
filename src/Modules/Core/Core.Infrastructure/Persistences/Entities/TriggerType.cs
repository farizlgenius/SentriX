using System.ComponentModel.DataAnnotations;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class TriggerType
{
      [Key]
      public int id { get; set; }
      public Guid guid {get; set;}
      public string name {get; set;} = string.Empty;
      public short value {get; set; }
      public ICollection<TriggerCode> codes {get; set; } = default!;
      public int? trigger_source_id {get; set;}
      public TriggerSource? trigger_source {get; set;}
      public TriggerType()
      {
            
      }
}