using SharedKernel.Enums;
using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class User : BaseDomain
{

      public string Username { get; private set; } = string.Empty;
      // public string Password {get; private set;} = string.Empty;
      public string Identification { get; private set; } = string.Empty;
      public Title Title { get; private set; } = Title.Mr;
      public string FirstName { get; private set; } = string.Empty;
      public string MiddleName { get; private set; } = string.Empty;
      public string LastName { get; private set; } = string.Empty;
      public Gender Gender { get; private set; } = Gender.M;
      public DateTime DateOfBirth { get; private set; }
      public string Email { get; private set; } = string.Empty;
      public string Phone { get; private set; } = string.Empty;
      public bool IsOperator { get; private set; } = false;
      public int? RoleGuid { get; private set; }
      public int? CompanyId { get; private set; }
      public int? DepartmentId { get; private set; }
      public int? PositionId { get; private set; }
      public string Address { get; private set; } = string.Empty;
      public DateTime ActiveTime { get; set; }
      public DateTime ExpireTime { get; set; }
      public List<string> Additionals { get; private set; } = new List<string>();
      public List<Guid> Groups { get; private set; } = new List<Guid>();
      public List<Card> Cards { get; private set; } = default!;
      public List<LicensePlate> LicensePlates { get; private set; } = default!;
      public List<Pin> Pins { get; private set; } = default!;
      public List<QrCode> QrCodes { get; private set; } = default!;
      public Face? Face { get; private set; }
      public List<int> LocationIds { get; private set; } = default!;

      public User(
    string Username,
    string Identification,
    Title Title,
    string Firstname,
    string Middlename,
    string Lastname,
    Gender Gender,
    DateTime DateOfBirth,
    string Email,
    string Phone,
    bool IsOperator,
    int RoleId,
    int CompanyId,
    int DepartmentId,
    int PositionId,
    string Address,
    DateTime ActiveTime,
    DateTime ExpireTime,
    List<string> Additionals,
    List<Guid> Groups,
    List<Card> Cards,
    List<LicensePlate> LicensePlates,
    List<Pin> Pins,
    List<QrCode> QrCodes,
    Face Face,
    List<int> LocationIds
)
      {
            ValidationHelper.Name(Firstname);
            ValidationHelper.CharAndDigit(Identification, nameof(this.Identification));
            ValidationHelper.Email(Email, nameof(Email));
            this.Username = Username;
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
            this.RoleGuid = RoleId;
            this.CompanyId = CompanyId;
            this.DepartmentId = DepartmentId;
            this.PositionId = PositionId;
            this.Address = Address;
            this.ActiveTime = ActiveTime;
            this.ExpireTime = ExpireTime;
            this.Additionals = Additionals;
            this.Groups = Groups;
            this.Cards = Cards;
            this.LicensePlates = LicensePlates;
            this.Pins = Pins;
            this.QrCodes = QrCodes;
            this.Face = Face;
            this.LocationIds = LocationIds;
      }

      public User(
     Guid Guid,
     string Username,
     string Identification,
     Title Title,
     string Firstname,
     string Middlename,
     string Lastname,
     Gender Gender,
     DateTime DateOfBirth,
     string Email,
     string Phone,
     bool IsOperator,
     int RoleId,
     int CompanyId,
     int DepartmentId,
     int PositionId,
     string Address,
     DateTime ActiveTime,
     DateTime ExpireTime,
     List<string> Additionals,
     List<Guid> Groups,
     List<Card> Cards,
     List<LicensePlate> LicensePlates,
     List<Pin> Pins,
     List<QrCode> QrCodes,
     Face Face,
     List<int> LocationIds
 ) : base(Guid)
      {
            this.Username = Username;
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
            this.RoleGuid = RoleId;
            this.CompanyId = CompanyId;
            this.DepartmentId = DepartmentId;
            this.PositionId = PositionId;
            this.Address = Address;
            this.ActiveTime = ActiveTime;
            this.ExpireTime = ExpireTime;
            this.Additionals = Additionals;
            this.Groups = Groups;
            this.Cards = Cards;
            this.LicensePlates = LicensePlates;
            this.Pins = Pins;
            this.QrCodes = QrCodes;
            this.Face = Face;
            this.LocationIds = LocationIds;
      }


}

