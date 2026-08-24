namespace Core.Domain.Entities;

public sealed class ModulePermission : BaseDomain
{
  public int RoleId { get; private set; }
  public int ModuleId { get; private set; }
  public bool IsEnabled { get; private set; }
  public List<FeaturePermission> FeaturePermissions { get; private set; } = default!;
  public ModulePermission(
        int RoleId,
        bool IsEnabled,
        List<FeaturePermission> featurePermissions
  )
  {
    this.ModuleId = RoleId;
    this.IsEnabled = IsEnabled;
    FeaturePermissions = featurePermissions;
  }
  public ModulePermission(
        Guid Guid,
        int RoleId,
        bool IsEnabled,
        List<FeaturePermission> featurePermissions
        ) : base(Guid)
  {
    this.ModuleId = RoleId;
    this.IsEnabled = IsEnabled;
    FeaturePermissions = featurePermissions;
  }
}