using System;
using SharedKernel.Domain;

namespace Role.Infrastructure.Persistences.Entities;

public sealed class Permission : BaseEntity
{
  public int role_id { get; set; }
  public Roles role { get; set; } = null!;
  public int feature_id { get; set; }
  public Feature feature { get; set; } = null!;
  public bool is_enabled { get; set; }
  public bool is_created { get; set; }
  public bool is_updated { get; set; }
  public bool is_deleted { get; set; }

  public Permission() { }

  public Permission(Domain.Entities.Permission domain)
  {
    feature_id = domain.Id;
    is_created = domain.IsCreated;
    is_enabled = domain.IsEnabled;
    is_deleted = domain.IsDeleted;
    is_updated = domain.IsUpdated;
    created_at = DateTime.UtcNow;
    updated_at = DateTime.UtcNow;
  }
  public Permission(int RoleId, Domain.Entities.Permission domain)
  {
    role_id = RoleId;
    feature_id = domain.Id;
    is_created = domain.IsCreated;
    is_enabled = domain.IsEnabled;
    is_deleted = domain.IsDeleted;
    is_updated = domain.IsUpdated;
    created_at = DateTime.UtcNow;
    updated_at = DateTime.UtcNow;
  }
  public void Update(int RoleId, Domain.Entities.Permission permission)
  {
    role_id = RoleId;
    feature_id = permission.Id;
    is_enabled = permission.IsEnabled;
    is_created = permission.IsCreated;
    is_updated = permission.IsUpdated;
    is_deleted = permission.IsDeleted;
    updated_at = DateTime.UtcNow;
  }
}
