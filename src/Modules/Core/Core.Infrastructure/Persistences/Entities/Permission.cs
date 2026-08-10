namespace Core.Infrastructure.Persistences.Entities;

public sealed class Permission : BaseEntity
{
      public Guid role_guid { get; set; } = default!;
      public Role role { get; set; } = default!;
      public Guid feature_guid { get; set; } = default!;
      public Feature feature { get; set; } = default!;
      public bool is_enabled { get; set; }
      public bool is_created { get; set; }
      public bool is_updated { get; set; }
      public bool is_deleted { get; set; }

      public Permission(){}
      public Permission(
            Core.Domain.Entities.Permission d
      ) : base(d.Guid)
      {
            this.role_guid = d.RoleGuid;
            this.feature_guid = d.FeatureGuid;
            this.is_deleted = d.IsEnabled;
            this.is_created = d.IsCreated;
            this.is_updated = d.IsUpdated;
            this.is_deleted = d.IsDeleted;
      }
}