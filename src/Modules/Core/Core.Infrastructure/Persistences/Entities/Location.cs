using System.ComponentModel.DataAnnotations;
namespace Core.Infrastructure.Persistences.Entities;

public sealed class Location : BaseEntity
{
  public string description { get; set; } = string.Empty;
  public int country_id { get; set; }
  public Country country { get; set; } = default!;
  public ICollection<Device> devices { get; set; } = default!;
  public ICollection<Module> modules { get; set; } = default!;
  public Location() { }

  public Location(Core.Domain.Entities.Location d)
  {
    name = d.Name;
    description = d.Description;
    country_id = d.CountryId;
    is_active = d.IsActive;
    is_default = d.IsDefault;
  }
}