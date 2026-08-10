namespace Core.Domain.Entities;

public sealed class Permission : BaseDomain
{
      public Guid RoleGuid { get; private set; }
      public Guid FeatureGuid { get; private set; }
      public bool IsEnabled { get; private set; }
      public bool IsCreated { get; private set; }
      public bool IsUpdated { get; private set; }
      public bool IsDeleted { get; private set; }
      public Permission(
            Guid RoleGuid,
            Guid FeatureGuid,
            bool IsEnabled,
            bool IsCreated,
            bool IsUpdated,
            bool IsDeleted
      )
      {
            this.RoleGuid = RoleGuid;
            this.FeatureGuid = FeatureGuid;
            this.IsEnabled = IsEnabled;
            this.IsCreated = IsCreated;
            this.IsUpdated = IsUpdated;
            this.IsDeleted = IsDeleted;
      }
      public Permission(
            Guid Guid
            ) : base(Guid)
      {
            this.RoleGuid = RoleGuid;
            this.FeatureGuid = FeatureGuid;
            this.IsEnabled = IsEnabled;
            this.IsCreated = IsCreated;
            this.IsUpdated = IsUpdated;
            this.IsDeleted = IsDeleted;
      }
}