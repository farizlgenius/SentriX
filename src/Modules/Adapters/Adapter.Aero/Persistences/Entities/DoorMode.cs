using System.ComponentModel.DataAnnotations;
using SharedKernel.Domain;

namespace Adapter.Aero.Persistences.Entities;

public sealed class DoorMode
{
      [Key]
      public int id { get; set; }
      public string label { get; set; } = string.Empty;
      public int value { get; set; }
      public string description { get; set; } = string.Empty;
      public DoorMode() { }

      public DoorMode(int id, string label, int value, string desc)
      {
            this.id = id;
            this.label = label;
            this.value = value;
            this.description = desc;
      }
}