namespace Core.Domain.Entities;

public sealed class Group : BaseDomain
{
    public string Name { get; private set; }
    public List<GroupComponent> Components { get; private set; }

    public Group(
        string name,
        List<GroupComponent> components
        ) : base(Guid.NewGuid())
    {
        Name = name;
        Components = components;
    }

    public Group(
        Guid Guid,
        string name,
        List<GroupComponent> components
        ) : base(Guid)
    {
        Name = name;
        Components = components;
    }
}