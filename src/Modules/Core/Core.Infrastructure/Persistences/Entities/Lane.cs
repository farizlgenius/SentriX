namespace Core.Infrastructure.Persistences.Entities;

public sealed class Lane : BaseEntity
{
      public int lane_no {get; set; }
      public ICollection<Reader> readers {get; set; }=  default!; 
      public Sensor? sensor {get; set;} 
      public ICollection<Relay> relays {get; set;} = default!;

      public Lane(){}
}