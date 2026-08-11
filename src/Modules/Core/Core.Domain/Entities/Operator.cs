namespace Core.Domain.Entities;

public sealed class Operator : BaseDomain
{
  public string Username { get; set; } = string.Empty;
  public string Password { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  public string Phone { get; set; } = string.Empty;
  public DateTime ActiveTime { get; set; } = DateTime.UtcNow;
  public DateTime ExpireTime { get; set; } = DateTime.UtcNow.AddYears(1);
  public Guid RoleGuid { get; set; }
  public List<Guid> LocationGuids { get; set; } = default!;

  public Operator(
    string userName,
    string password,
    string email,
    string phone,
    DateTime active,
    DateTime expire,
    Guid roleGuid,
    List<Guid> locationGuids
  )
  {
    Username = userName;
    Password = password;
    Email = email;
    Phone = phone;
    ActiveTime = active;
    ExpireTime = expire;
    RoleGuid = roleGuid;
    LocationGuids = locationGuids;
  }

  public Operator(
    Guid Guid,
    string userName,
    string password,
    string email,
    string phone,
    DateTime active,
    DateTime expire,
    Guid roleGuid,
    List<Guid> locationGuids
  ) : base(Guid)
  {
    Username = userName;
    Password = password;
    Email = email;
    Phone = phone;
    ActiveTime = active;
    ExpireTime = expire;
    RoleGuid = roleGuid;
    LocationGuids = locationGuids;
  }
}