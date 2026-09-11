namespace Core.Infrastructure.Persistences.Entities;

public sealed class Lane : BaseEntity
{
      public int lane_no { get; set; }
      public int turnstile_id { get; set; }
      public Turnstile turnstile { get; set; } = default!;
      public ICollection<Reader> readers { get; set; } = default!;
      public int? sensor_id { get; set; }
      public Sensor? sensor { get; set; }
      public Lane() { }
      public Lane(Domain.Entities.Lane d) : base(d.Guid)
      {
            lane_no = d.LaneNo;
            readers = d.Readers.Select(x => new Reader(x)).ToArray();
            sensor = d.Sensor == null ? null : new Sensor(d.Sensor);
      }

}