namespace Core.Infrastructure.Persistences.Entities;

public sealed class Group : BaseEntity
{
  public string name { get; set; } = string.Empty;
  public ICollection<GroupComponent> components { get; set; } = default!;
  public ICollection<UserGroup> user_groups { get; set; } = default!;
  public int location_id { get; set; }
  public Location location { get; set; } = default!;

  public Group() { }
  public Group(
    Domain.Entities.Group d
  ) : base(d.Guid)
  {
    name = d.Name;
    components = d.Components.Select(x => new GroupComponent(x)).ToArray();
  }

}