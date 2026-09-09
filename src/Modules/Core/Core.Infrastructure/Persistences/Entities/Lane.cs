namespace Core.Infrastructure.Persistences.Entities;

public sealed class Lane : BaseEntity
{
      public int lane_no { get; set; }
      public ICollection<Reader> readers { get; set; } = default!;
      public Sensor? sensor { get; set; }
      public ICollection<Relay> relays { get; set; } = default!;
      public Lane() { }
      public Lane(Domain.Entities.Lane d) : base(d.Guid)
      {
            lane_no = d.LaneNo;
            readers = d.Readers.Select(x => new Reader(x)).ToArray();
            sensor = d.Sensor == null ? null : new Sensor(d.Sensor);
            relays = d.Relays.Select(x => new Relay(x)).ToArray();
      }

}