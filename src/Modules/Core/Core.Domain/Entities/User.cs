using SharedKernel.Enums;
using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class User : BaseDomain
{
      public string UserCode { get; private set; } = string.Empty;
      public string Username { get; private set; } = string.Empty;
      public string Password { get; private set; } = string.Empty;
      public string Identification { get; private set; } = string.Empty;
      public Title Title { get; private set; } = Title.Mr;
      public string FirstName { get; private set; } = string.Empty;
      public string MiddleName { get; private set; } = string.Empty;
      public string LastName { get; private set; } = string.Empty;
      public Gender Gender { get; private set; } = Gender.Male;
      public DateTime DateOfBirth { get; private set; }
      public string Email { get; private set; } = string.Empty;
      public string Phone { get; private set; } = string.Empty;
      public bool IsOperator { get; private set; } = false;
      public bool IsUser { get; private set; } = true;
      public int? RoleId { get; private set; }
      public int? CompanyId { get; private set; }
      public int? DepartmentId { get; private set; }
      public int? PositionId { get; private set; }
      public string Address { get; private set; } = string.Empty;
      public DateTime JoinedTime { get; set; }
      public DateTime ExpiredTime { get; set; }
      public List<string> Additionals { get; private set; } = new List<string>();
      public List<Guid> Groups { get; private set; } = new List<Guid>();
      public List<Card> Cards { get; private set; } = default!;
      public LicensePlate? LicensePlate { get; private set; } = default!;
      public Pin? Pin { get; private set; } = default!;
      public QrCode? QrCode { get; private set; } = default!;
      public Face? Face { get; private set; } = default!;
      public List<int> LocationIds { get; private set; } = default!;
      public List<int> GroupIds { get; private set; } = default!;

      public User(
            string UserCode,
    string Username,
    string Password,
    string Identification,
    Title Title,
    string Firstname,
    string Middlename,
    string Lastname,
    Gender Gender,
    DateTime DateOfBirth,
    string Email,
    string Phone,
    string Address,
    DateTime ActiveTime,
    DateTime ExpireTime,
    List<string> Additionals,
    List<int> LocationIds,
    List<int> GroupIds,
    bool IsOperator,
    bool IsUser,
    int RoleId,
    int CompanyId,
    int DepartmentId,
    int PositionId,
    List<Card> Cards,
    LicensePlate? LicensePlate = null,
    Pin? Pin = null,
    QrCode? QrCode = null
)
      {
            ValidationHelper.Name(Firstname);
            ValidationHelper.CharAndDigit(Identification, nameof(this.Identification));
            ValidationHelper.Email(Email, nameof(Email));
            ValidationHelper.IsNullOrEmpty(Password, nameof(Password));
            ValidationHelper.CharAndDigit(UserCode, nameof(UserCode));
            this.Username = Username;
            if (!string.IsNullOrWhiteSpace(Password))
            {
                  this.Password = PasswordHasher.HashPassword(Password);
            }
            this.Identification = Identification;
            this.Title = Title;
            this.FirstName = Firstname;
            this.MiddleName = Middlename;
            this.LastName = Lastname;
            this.Gender = Gender;
            this.DateOfBirth = DateOfBirth;
            this.Email = Email;
            this.Phone = Phone;
            this.IsOperator = IsOperator;
            this.IsUser = IsUser;
            this.RoleId = RoleId;
            this.CompanyId = CompanyId;
            this.DepartmentId = DepartmentId;
            this.PositionId = PositionId;
            this.Address = Address;
            this.JoinedTime = ActiveTime;
            this.ExpiredTime = ExpireTime;
            this.Additionals = Additionals;
            this.Groups = Groups;
            this.Cards = Cards;
            this.LicensePlate = LicensePlate;
            this.Pin = Pin;
            this.QrCode = QrCode;
            this.LocationIds = LocationIds;
            this.GroupIds = GroupIds;
            this.UserCode = UserCode;
      }

      public User(
     Guid Guid,
     string UserCode,
     string Username,
     string Password,
    string Identification,
    Title Title,
    string Firstname,
    string Middlename,
    string Lastname,
    Gender Gender,
    DateTime DateOfBirth,
    string Email,
    string Phone,
    string Address,
    DateTime ActiveTime,
    DateTime ExpireTime,
    List<string> Additionals,
    List<int> LocationIds,
    List<int> GroupIds,
    bool IsOperator,
    bool IsUser,
    int RoleId,
    int CompanyId,
    int DepartmentId,
    int PositionId,
    List<Card> Cards,
    LicensePlate? LicensePlate = null,
    Pin? Pin = null,
    QrCode? QrCode = null
 ) : base(Guid)
      {
            ValidationHelper.Name(Firstname);
            ValidationHelper.CharAndDigit(Identification, nameof(this.Identification));
            ValidationHelper.Email(Email, nameof(Email));
            ValidationHelper.IsNullOrEmpty(Password, nameof(Password));
            ValidationHelper.CharAndDigit(UserCode, nameof(UserCode));
            this.Username = Username;
            if (!string.IsNullOrWhiteSpace(Password))
            {
                  this.Password = PasswordHasher.HashPassword(Password);
            }
            this.Identification = Identification;
            this.Title = Title;
            this.FirstName = Firstname;
            this.MiddleName = Middlename;
            this.LastName = Lastname;
            this.Gender = Gender;
            this.DateOfBirth = DateOfBirth;
            this.Email = Email;
            this.Phone = Phone;
            this.IsOperator = IsOperator;
            this.IsUser = IsUser;
            this.RoleId = RoleId;
            this.CompanyId = CompanyId;
            this.DepartmentId = DepartmentId;
            this.PositionId = PositionId;
            this.Address = Address;
            this.JoinedTime = ActiveTime;
            this.ExpiredTime = ExpireTime;
            this.Additionals = Additionals;
            this.Groups = Groups;
            this.Cards = Cards;
            this.LicensePlate = LicensePlate;
            this.Pin = Pin;
            this.QrCode = QrCode;
            this.LocationIds = LocationIds;
            this.GroupIds = GroupIds;
            this.UserCode = UserCode;
      }


}

