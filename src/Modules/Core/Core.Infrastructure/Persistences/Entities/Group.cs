using SharedKernel.Interfaces;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Group : BaseEntity,IAuditableEntity
{
  public ICollection<GroupComponent> components { get; set; } = default!;
  public ICollection<UserGroup> user_groups { get; set; } = default!;

  public Group() { }
  public Group(
    Domain.Entities.Group d
  ) : base(d.Guid)
  {
    name = d.Name;
    components = d.Components.Select(x => new GroupComponent(x)).ToArray();
    if(d.LocationId != 0)
      location_id = d.LocationId;
  }

}