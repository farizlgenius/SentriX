namespace Core.Domain.Entities;

public sealed class FeaturePermission : BaseDomain
{
  public int FeatureId { get; private set; }
  public bool IsEnabled { get; private set; }
  public bool IsCreated { get; private set; }
  public bool IsUpdated { get; private set; }
  public bool IsDeleted { get; private set; }
  public FeaturePermission(
        int FeatureId,
        bool IsEnabled,
        bool IsCreated,
        bool IsUpdated,
        bool IsDeleted
  )
  {
    this.FeatureId = FeatureId;
    this.IsEnabled = IsEnabled;
    this.IsCreated = IsCreated;
    this.IsUpdated = IsUpdated;
    this.IsDeleted = IsDeleted;
  }
  public FeaturePermission(
        Guid Guid
        ) : base(Guid)
  {
    this.FeatureId = FeatureId;
    this.IsEnabled = IsEnabled;
    this.IsCreated = IsCreated;
    this.IsUpdated = IsUpdated;
    this.IsDeleted = IsDeleted;
  }
}