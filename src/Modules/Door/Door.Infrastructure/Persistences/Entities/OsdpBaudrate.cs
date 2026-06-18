using SharedKernel.Domain;

namespace Door.Infrastructure.Persistences.Entities;

public sealed class OsdpBaudrate : BaseEntity
{
      public string label {get; set;} = string.Empty;
      public int value {get; set;} 
      public string description {get; set;} = string.Empty;

      public OsdpBaudrate(){}
      public OsdpBaudrate(string label,int value,string description) : base(0,0,true)
      {
            this.label = label;
            this.value = value;
            this.description = description;
      }
}