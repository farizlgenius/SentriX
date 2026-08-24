namespace Core.Infrastructure.Persistences.Entities;

public sealed class Role : BaseEntity
{
      public string name { get; set; } = string.Empty;

      // Relation
      public ICollection<User> users { get; set; } = default!;
      public ICollection<ModulePermission> module_permission { get; set; } = default!;
      public int location_id { get; set; } = default!;
      public Location location { get; set; } = default!;

      public Role() { }
      public Role(Core.Domain.Entities.Role d) : base(d.Guid)
      {
            name = d.Name;
            location_id = d.LocationId;

      }
}