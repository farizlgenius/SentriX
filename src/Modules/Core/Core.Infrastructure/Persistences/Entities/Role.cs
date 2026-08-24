namespace Core.Infrastructure.Persistences.Entities;

public sealed class Role : BaseEntity
{
      public string name { get; set; } = string.Empty;

      // Releation
      public ICollection<Operator> operators { get; set; } = default!;
      public ICollection<Permission> permissions { get; set; } = default!;
      public int location_id { get; set; } = default!;
      public Location location { get; set; } = default!;

      public Role() { }
      public Role(Core.Domain.Entities.Role d) : base(d.Guid)
      {
            name = d.Name;
            permissions = d.Permissions.Select(x => new Permission(x)).ToArray();
            location_id = d.LocationId;

      }
}