namespace Core.Domain.Entities;

public sealed class Group : BaseDomain
{
    public string Name { get; private set; }
    public List<GroupComponent> Components { get; private set; }
    public int LocationId { get; private set; }

    public Group(
        string name,
        List<GroupComponent> components,
        int locationId
        ) : base(Guid.NewGuid())
    {
        Name = name;
        Components = components;
        LocationId = locationId;
    }

    public Group(
        Guid Guid,
        string name,
        List<GroupComponent> components,
         int locationId
        ) : base(Guid)
    {
        Name = name;
        Components = components;
        LocationId = locationId;
    }
}