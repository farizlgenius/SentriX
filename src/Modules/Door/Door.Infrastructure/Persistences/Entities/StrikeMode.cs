using SharedKernel.Domain;

namespace Door.Infrastructure.Persistences.Entities;

public sealed class StrikeMode : BaseEntity
{
      public string label {get; set;} = string.Empty;
      public short value {get; set;} 
      public string description {get; set;} = string.Empty;

      public StrikeMode(){}
      public StrikeMode(string label,short value,string description) : base(0,0,true,false)
      {
            this.label = label;
            this.value = value;
            this.description = description;
      }
}