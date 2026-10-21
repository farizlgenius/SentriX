namespace Core.Infrastructure.Persistences.Entities;

public sealed class Module : BaseEntity
{
  public ICollection<Feature> features { get; set; } = default!;
  public ICollection<ModulePermission> module_permissions { get; set; } = default!;
  public Module()
  { }
}