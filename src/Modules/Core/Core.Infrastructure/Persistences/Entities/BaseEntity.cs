using System.ComponentModel.DataAnnotations;

namespace Core.Infrastructure.Persistences.Entities;

public abstract class BaseEntity
{
  [Key]
  public int id { get; set; }
  public Guid guid { get; set; }
  public string vendor {get ;set;} = string.Empty;  
  public DateTime created_at { get; set; }
  public DateTime updated_at { get; set; }
  public bool is_active { get; set; } = true;
  public bool is_default { get; set; } = false;

  // Relationship with Location
  public Guid location_guid { get; set; }
  public Location location { get; set; } = default!;

  public BaseEntity() { }

  public BaseEntity(Guid guid, Guid location_guid, bool is_active, bool is_default)
  {
    this.guid = guid;
    this.location_guid = location_guid;
    this.is_active = is_active;
    this.is_default = is_default;
  }

  public BaseEntity(Guid guid, Guid location_guid, bool is_active, bool is_default, string vendor)
  {
    this.guid = guid;
    this.location_guid = location_guid;
    this.is_active = is_active;
    this.is_default = is_default;
    this.vendor = vendor;
  }
}