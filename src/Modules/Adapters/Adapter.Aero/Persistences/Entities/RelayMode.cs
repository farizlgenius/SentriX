using System.ComponentModel.DataAnnotations;

namespace Adapter.Aero.Persistences.Entities;

public sealed class RelayMode
{
      [Key]
      public int id {get; set;}
      public string label {get; set;} = string.Empty;
      public int value { get; set; }
      public RelayMode(){}

      public RelayMode(int id, string label, int value)
      {
            this.id = id;
            this.label = label;
            this.value = value;
      }
}