namespace Core.Infrastructure.Persistences.Entities;

public sealed class Lane : BaseEntity
{
      public int lane_no { get; set; }
      public int turnstile_id { get; set; }
      public Turnstile turnstile { get; set; } = default!;
      public ICollection<Door> doors
      { get; set; } = default!;
      public Lane() { }
      public Lane(Domain.Entities.Lane d) : base(d.Guid)
      {
            lane_no = d.LaneNo;
            doors = d.Doors.Select(x => new Door(x)).ToArray();
      }

}