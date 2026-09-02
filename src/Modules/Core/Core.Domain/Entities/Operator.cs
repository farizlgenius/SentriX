using SharedKernel.Enums;
using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class Operator : BaseDomain
{
  public string Username { get; set; } = string.Empty;
  public string Password { get; set; } = string.Empty;
  public Title Title { get; set; } = Title.Mr;
  public string Firstname { get; set; } = string.Empty;
  public string Middlename { get; set; } = string.Empty;
  public string Lastname { get; set; } = string.Empty;
  public Gender Gender { get; set; } = Gender.Male;
  public string Email { get; set; } = string.Empty;
  public string Phone { get; set; } = string.Empty;
  public DateTime JoinedDate { get; set; } = DateTime.UtcNow;
  public DateTime ExpiredDate { get; set; } = DateTime.UtcNow.AddYears(1);
  public int RoleId { get; set; }
  public List<int> LocationIds { get; set; } = default!;

  public Operator(
    string userName,
    string password,
    Title title,
    string firstname,
    string middlename,
    string lastname,
    Gender gender,
    string email,
    string phone,
    DateTime join,
    DateTime expire,
    int roleId,
    List<int> locationIds
  )
  {
    ValidationHelper.CharAndDigit(userName, nameof(Username));
    ValidationHelper.IsNullOrEmpty(password, nameof(Password));
    ValidationHelper.Email(email, nameof(Email));
    ValidationHelper.ValidateActiveTime(join, expire);
    Username = userName;
    Password = PasswordHasher.HashPassword(password);
    Title = title;
    Firstname = firstname;
    Middlename = middlename;
    Lastname = lastname;
    Gender = gender;
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
    string password,
    Title title,
    string firstname,
    string middlename,
    string lastname,
    Gender gender,
    string email,
    string phone,
    DateTime join,
    DateTime expire,
    int roleId,
    List<int> locationIds
  ) : base(Guid)
  {
    ValidationHelper.CharAndDigit(userName, nameof(Username));
    ValidationHelper.IsNullOrEmpty(password, nameof(Password));
    ValidationHelper.Email(email, nameof(Email));
    ValidationHelper.ValidateActiveTime(join, expire);
    Username = userName;
    Password = PasswordHasher.HashPassword(password);
    Title = title;
    Firstname = firstname;
    Middlename = middlename;
    Lastname = lastname;
    Gender = gender;
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
    Title title,
    string firstname,
    string middlename,
    string lastname,
    Gender gender,
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
    Title = title;
    Firstname = firstname;
    Middlename = middlename;
    Lastname = lastname;
    Gender = gender;
    Email = email;
    Phone = phone;
    JoinedDate = join;
    ExpiredDate = expire;
    RoleId = roleId;
    LocationIds = locationIds;
  }


}