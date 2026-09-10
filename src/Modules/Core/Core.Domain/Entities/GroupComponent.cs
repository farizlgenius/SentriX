namespace Core.Domain.Entities;

public sealed class GroupComponent : BaseDomain
{
    public int DoorId { get; private set; }
    public int TimeZoneId { get; private set; }

    public GroupComponent(int doorId, int timeZoneId) : base(Guid.NewGuid())
    {
        DoorId = doorId;
        TimeZoneId = timeZoneId;
    }

    public GroupComponent(Guid guid, int doorId, int timeZoneId) : base(guid)
    {
        DoorId = doorId;
        TimeZoneId = timeZoneId;
    }
}