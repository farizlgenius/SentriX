using System;

namespace Role.Domain.Entities;

public sealed class Roles
{
  public int Id { get; private set; }
  public string Name { get; private set; } = string.Empty;
  public List<Permission> Permissions { get; private set; } = new List<Permission>();
  public int LocationId { get; private set; }

  public Roles(int id, string name, List<Permission> permissions, int locationId)
  {
    Id = id;
    Name = name;
    Permissions = permissions;
    LocationId = locationId;
  }

}
