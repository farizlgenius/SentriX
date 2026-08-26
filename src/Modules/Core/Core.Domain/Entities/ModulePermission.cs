namespace Core.Domain.Entities;

public sealed class ModulePermission : BaseDomain
{
  public bool IsEnabled { get; private set; }
  public List<FeaturePermission> FeaturePermissions { get; private set; } = default!;
  public ModulePermission(
        bool IsEnabled,
        List<FeaturePermission> featurePermissions
  )
  {
    this.IsEnabled = IsEnabled;
    FeaturePermissions = featurePermissions;
  }
  public ModulePermission(
        Guid Guid,
        bool IsEnabled,
        List<FeaturePermission> featurePermissions
        ) : base(Guid)
  {
    this.IsEnabled = IsEnabled;
    FeaturePermissions = featurePermissions;
  }
}