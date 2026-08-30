namespace Core.Infrastructure.Persistences.Entities;

public sealed class FeaturePermission : BaseEntity
{
      public int feature_id { get; set; } = default!;
      public Feature feature { get; set; } = default!;
      public int module_permission_id { get; set; }
      public ModulePermission module_permission { get; set; } = default!;
      public bool is_enabled { get; set; }
      public bool is_created { get; set; }
      public bool is_updated { get; set; }
      public bool is_deleted { get; set; }

      public FeaturePermission() { }
      public FeaturePermission(
            Core.Domain.Entities.FeaturePermission d
      ) : base(d.Guid)
      {
            this.feature_id = d.FeatureId;
            this.is_enabled = d.IsEnabled;
            this.is_deleted = d.IsEnabled;
            this.is_created = d.IsCreated;
            this.is_updated = d.IsUpdated;
            this.is_deleted = d.IsDeleted;
      }
}