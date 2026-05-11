using System;
using SharedKernel.Domain;

namespace Role.Infrastructure.Persistences.Entities;

public sealed class Roles : BaseEntity
{
  public string name { get; set; } = string.Empty;

  // Relation here
  public int location_id { get; set; }
  public ICollection<RoleOperator> role_operators { get; set; } = new List<RoleOperator>();
  public ICollection<Permission> permissions { get; set; } = new List<Permission>();
  public Roles() { }
  public Roles(Domain.Entities.Roles domain)
  {
    name = domain.Name;
    location_id = domain.LocationId;
    permissions = domain.Permissions.Select(p => new Persistences.Entities.Permission(p)).ToList();
    created_at = DateTime.UtcNow;
    updated_at = DateTime.UtcNow;
  }
  public void AddPermissions(List<Domain.Entities.Permission> permissions)
  {
    this.permissions = permissions.Select(p => new Persistences.Entities.Permission(this.id, p)).ToList(); ;
  }
  public void Update(Domain.Entities.Roles role)
  {
    name = role.Name;
    location_id = role.LocationId;
    permissions = role.Permissions.Select(p => new Persistences.Entities.Permission(role.Id, p)).ToList();
    updated_at = DateTime.UtcNow;
  }
}
