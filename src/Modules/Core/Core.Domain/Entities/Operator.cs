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
  public int RoleId { get; set; }
  public List<int> LocationIds { get; set; } = default!;

  public Operator(
    string userName,
    string password,
    string email,
    string phone,
    DateTime join,
    DateTime expire,
    int roleGuid,
    List<int> locationGuids
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
    RoleId = roleGuid;
    LocationIds = locationGuids;
  }

  public Operator(
    Guid Guid,
    string userName,
    string password,
    string email,
    string phone,
    DateTime join,
    DateTime expire,
    int roleId,
    List<int> locationIds
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
    RoleId = roleId;
    LocationIds = locationIds;
  }

  public Operator(
    Guid Guid,
    string userName,
    string email,
    string phone,
    DateTime join,
    DateTime expire,
    int roleId,
    List<int> locationIds
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
    RoleId = roleId;
    LocationIds = locationIds;
  }
}