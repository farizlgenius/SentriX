using System.ComponentModel.DataAnnotations;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class TriggerCode
{
      [Key]
      public int id { get; set; }
      public Guid guid {get; set;}
      public string name {get; set;} = string.Empty;
      public short value {get; set; }
      public int? trigger_type_id {get; set;}
      public TriggerType? trigger_type {get; set;}
      public TriggerCode()
      {
      
      }
}