using System.ComponentModel.DataAnnotations;
namespace Core.Infrastructure.Persistences.Entities;

public sealed class Location : BaseEntity
{
  public string name { get; set; } = string.Empty;
  public string description { get; set; } = string.Empty;
  public int country_id { get; set; }
  public Country country { get; set; } = default!;
  public ICollection<Device> devices { get; set; } = default!;
  public ICollection<DeviceModule> modules { get; set; } = default!;
  public ICollection<Company> companies { get; set; } = default!;
  public ICollection<User> users { get; set; } = default!;
  public ICollection<Role> roles { get; set; } = default!;
  public ICollection<TimeZone> timezones { get; set; } = default!;
  public ICollection<Holiday> holidays { get; set; } = default!;
  public ICollection<UserLocation> user_locations { get; set; } = default!;
  public ICollection<OperatorLocation> operator_locations { get; set; } = default!;
  public ICollection<ComponentMapping> component_mapping { get; set; } = default!;
  public ICollection<Interval> intervals { get; set; } = default!;
  public ICollection<Door> doors { get; set; } = default!;
  public ICollection<Group> groups { get; set; } = default!;
  public ICollection<Output> outputs { get; set; } = default!;
  public ICollection<Turnstile> turnstiles { get; set; } = default!;
  public ICollection<Event> events { get; set; } = default!;
  public ICollection<AdapterEvent> adapter_events { get; set; } = default!;
  public ICollection<AuditTrail> audit_trails { get; set; } = default!;
  public Location() { }
  public Location(Core.Domain.Entities.Location d) : base(d.Guid)
  {
    this.name = d.Name;
    this.description = d.Description;
    this.country_id = d.CountryId;

  }


}