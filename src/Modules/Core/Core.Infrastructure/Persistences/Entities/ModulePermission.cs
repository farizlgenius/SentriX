namespace Core.Infrastructure.Persistences.Entities;

public sealed class ModulePermission : BaseEntity
{
  public int module_id { get; set; }
  public Module module { get; set; } = default!;
  public bool is_enabled { get; set; }
  public ICollection<FeaturePermission> feature_permissions { get; set; } = default!;
  public int role_id { get; set; }
  public Role role { get; set; } = default!;
  public ModulePermission()
  { }

  public ModulePermission(Domain.Entities.ModulePermission d)
  {
    module_id = d.ModuleId;
    is_enabled = d.IsEnabled;
    role_id = d.RoleId;
    feature_permissions = d.FeaturePermissions.Select(
      x => new Persistences.Entities.FeaturePermission(x)
    ).ToArray();
  }
}