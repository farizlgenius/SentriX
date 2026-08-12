using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class Operator : BaseDomain
{
  public string Username { get; set; } = string.Empty;
  public string Password { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  public string Phone { get; set; } = string.Empty;
  public DateTime JoinedDate { get; set; } = DateTime.UtcNow;
  public DateTime ExpiredDate { get; set; } = DateTime.UtcNow.AddYears(1);
  public Guid RoleGuid { get; set; }
  public List<Guid> LocationGuids { get; set; } = default!;

  public Operator(
    string userName,
    string password,
    string email,
    string phone,
    DateTime join,
    DateTime expire,
    Guid roleGuid,
    List<Guid> locationGuids
  )
  {
    ValidationHelper.CharAndDigit(userName, nameof(Username));
    ValidationHelper.Email(email, nameof(Email));
    ValidationHelper.ValidateActiveTime(join, expire);
    Username = userName;
    Password = PasswordHasher.HashPassword(password);
    Email = email;
    Phone = phone;
    JoinedDate = join;
    ExpiredDate = expire;
    RoleGuid = roleGuid;
    LocationGuids = locationGuids;
  }

  public Operator(
    Guid Guid,
    string userName,
    string password,
    string email,
    string phone,
    DateTime join,
    DateTime expire,
    Guid roleGuid,
    List<Guid> locationGuids
  ) : base(Guid)
  {
    ValidationHelper.CharAndDigit(userName, nameof(Username));
    ValidationHelper.Email(email, nameof(Email));
    ValidationHelper.ValidateActiveTime(join, expire);
    Username = userName;
    Password = PasswordHasher.HashPassword(password);
    Email = email;
    Phone = phone;
    JoinedDate = join;
    ExpiredDate = expire;
    RoleGuid = roleGuid;
    LocationGuids = locationGuids;
  }

  public Operator(
    Guid Guid,
    string userName,
    string email,
    string phone,
    DateTime join,
    DateTime expire,
    Guid roleGuid,
    List<Guid> locationGuids
  ) : base(Guid)
  {
    ValidationHelper.CharAndDigit(userName, nameof(Username));
    ValidationHelper.Email(email, nameof(Email));
    ValidationHelper.ValidateActiveTime(join, expire);
    Username = userName;
    Password = string.Empty;
    Email = email;
    Phone = phone;
    JoinedDate = join;
    ExpiredDate = expire;
    RoleGuid = roleGuid;
    LocationGuids = locationGuids;
  }
}