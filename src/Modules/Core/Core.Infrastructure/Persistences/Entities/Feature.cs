using System.ComponentModel.DataAnnotations;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Feature 
{
      [Key]
      public int id {get; set ;}
      public Guid guid {get; set; }
      public string name {get; set; } = string.Empty;

      // Releation
      public int module_id { get; set; }
      public Module module { get; set; } = default!;
      public ICollection<FeaturePermission> feature_permission { get; set; } = default!;
      public Feature() { }
}