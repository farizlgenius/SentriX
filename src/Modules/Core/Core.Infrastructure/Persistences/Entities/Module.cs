namespace Core.Infrastructure.Persistences.Entities;

public sealed class Module : BaseEntity
{
  public string name { get; set; } = string.Empty;
  public ICollection<Feature> features { get; set; } = default!;
  public ICollection<ModulePermission> module_permissions { get; set; } = default!;
  public Module()
  { }
}