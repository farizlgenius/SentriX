namespace Core.Infrastructure.Persistences.Entities;

public sealed class GroupComponent : BaseEntity
{
    public int door_id { get; set; }
    public Door door { get; set; } = default!;
    public int timezone_id { get; set; }
    public TimeZone timezone { get; set; } = default!;
    public int group_id { get; set; }
    public Group group { get; set; } = default!;

    public GroupComponent() { }
    public GroupComponent(Domain.Entities.GroupComponent d) : base(d.Guid)
    {
        door_id = d.DoorId;
        timezone_id = d.TimeZoneId;
    }
}