namespace Core.Infrastructure.Persistences.Entities;

public sealed class Operator : BaseEntity
{
  public string username { get; set; } = string.Empty;
  public string password { get; set; } = string.Empty;
  public string email { get; set; } = string.Empty;
  public string phone { get; set; } = string.Empty;
  public DateTime active_time { get; set; }
  public DateTime expire_time { get; set; }

  // Relation
  public Guid role_guid { get; set; }
  public Role role { get; set; } = default!;
  public ICollection<OperatorLocation> operator_locations { get; set; } = default!;

  public Operator() { }
  public Operator(Core.Domain.Entities.Operator d) : base(d.Guid)
  {
    username = d.Username;
    password = d.Password;
    email = d.Email;
    phone = d.Phone;
    active_time = d.ActiveTime;
    expire_time = d.ExpireTime;
    role_guid = d.RoleGuid;
    operator_locations = d.LocationGuids.Select(x => new OperatorLocation(
      d.Guid,
      x
    )).ToArray();
  }
}