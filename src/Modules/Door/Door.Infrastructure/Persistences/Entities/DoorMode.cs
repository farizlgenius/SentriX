using SharedKernel.Domain;

namespace Door.Infrastructure.Persistences.Entities;

public sealed class DoorMode : BaseEntity
{
      public string label {get; set;} = string.Empty;
      public short value {get; set;} 
      public string description {get; set;} = string.Empty;

      public DoorMode(){}
      public DoorMode(string label,short value,string description) : base(0,0,true)
      {
            this.label = label;
            this.value = value;
            this.description = description;
      }
}