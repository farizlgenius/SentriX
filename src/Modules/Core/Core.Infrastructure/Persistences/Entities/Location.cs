using System.ComponentModel.DataAnnotations;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Location
{
  [Key]
  public int id { get; set; }
  public Guid guid { get; set; }
  public string name { get; set; } = string.Empty;
  public string description { get; set; } = string.Empty;
  public DateTime created_at { get; set; }
  public DateTime updated_at { get; set; }
  public int country_id { get; set; }
  public Country country { get; set; } = default!;
  public bool is_active { get; set; } = true;
  public bool is_default { get; set; } = false;
  public ICollection<Device> devices   { get; set; } = default!;
  public ICollection<Module> modules   { get; set; } = default!;
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