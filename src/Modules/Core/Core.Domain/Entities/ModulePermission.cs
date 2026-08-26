namespace Core.Domain.Entities;

public sealed class ModulePermission : BaseDomain
{
  public bool IsEnabled { get; private set; }
  public int ModuleId { get; private set; }
  public List<FeaturePermission> FeaturePermissions { get; private set; } = default!;
  public ModulePermission(
        bool IsEnabled,
        int ModuleId,
        List<FeaturePermission> featurePermissions
  )
  {
    this.IsEnabled = IsEnabled;
    this.ModuleId = ModuleId;
    FeaturePermissions = featurePermissions;
  }
  public ModulePermission(
        Guid Guid,
        bool IsEnabled,
        int ModuleId,
        List<FeaturePermission> featurePermissions
        ) : base(Guid)
  {
    this.IsEnabled = IsEnabled;
    this.ModuleId = ModuleId;
    FeaturePermissions = featurePermissions;
  }
}