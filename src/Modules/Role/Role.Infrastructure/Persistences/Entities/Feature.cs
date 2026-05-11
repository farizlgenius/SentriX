using System;
using SharedKernel.Domain;

namespace Role.Infrastructure.Persistences.Entities;

public sealed class Feature : BaseEntity
{
  public string name { get; set; } = string.Empty;
  public ICollection<Permission> permissions { get; set; } = new List<Permission>();
  public Feature() { }

}
