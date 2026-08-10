using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class User : BaseDomain
{

      public string Username { get; private set; } = string.Empty;
      // public string Password {get; private set;} = string.Empty;
      public string Identification { get; private set; } = string.Empty;
      public string Title { get; private set; } = string.Empty;
      public string FirstName { get; private set; } = string.Empty;
      public string MiddleName { get; private set; } = string.Empty;
      public string LastName { get; private set; } = string.Empty;
      public string Gender { get; private set; } = string.Empty;
      public DateTime DateOfBirth { get; private set; }
      public string Email { get; private set; } = string.Empty;
      public string Phone { get; private set; } = string.Empty;
      public bool IsOperator { get; private set; } = false;
      public Guid? RoleGuid { get; private set; }
      public Guid? CompanyGuid { get; private set; }
      public Guid? DepartmentGuid { get; private set; }
      public Guid? PositionGuid { get; private set; }
      public string Address { get; private set; } = string.Empty;
      public DateTime ActiveTime { get; set; }
      public DateTime ExpireTime { get; set; }
      public List<string> Additionals { get; private set; } = new List<string>();
      public List<Guid> Groups { get; private set; } = new List<Guid>();
      public List<Card> Cards {get; private set;} = default!;
      public List<LicensePlate> LicensePlates { get; private set; } = default!;
      public List<Pin> Pins { get; private set; } = default!;
      public List<QrCode> QrCodes {get; private set;} = default!;
      public Face? Face { get; private set; }

      public User(
    string Username,
    string Identification,
    string Title,
    string Firstname,
    string Middlename,
    string Lastname,
    string Gender,
    DateTime DateOfBirth,
    string Email,
    string Phone,
    bool IsOperator,
    Guid RoleGuid,
    Guid CompanyGuid,
    Guid DepartmentGuid,
    Guid PositionGuid,
    string Address,
    DateTime ActiveTime,
    DateTime ExpireTime,
    List<string> Additionals,
    List<Guid> Groups,
    List<Card> Cards,
    List<LicensePlate> LicensePlates,
    List<Pin> Pins,
    List<QrCode> QrCodes,
    Face Face
)
      {
            ValidationHelper.IsValidName(Firstname);
            ValidationHelper.IsValidOnlyCharAndDigit(Identification,nameof(this.Identification));
            ValidationHelper.IsValidEmail(Email,nameof(Email));
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
            this.RoleGuid = RoleGuid;
            this.CompanyGuid = CompanyGuid;
            this.DepartmentGuid = DepartmentGuid;
            this.PositionGuid = PositionGuid;
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
      }

      public User(
     Guid Guid,
     string Username,
     string Identification,
     string Title,
     string Firstname,
     string Middlename,
     string Lastname,
     string Gender,
     DateTime DateOfBirth,
     string Email,
     string Phone,
     bool IsOperator,
     Guid RoleGuid,
     Guid CompanyGuid,
     Guid DepartmentGuid,
     Guid PositionGuid,
     string Address,
     DateTime ActiveTime,
     DateTime ExpireTime,
     List<string> Additionals,
     List<Guid> Groups,
     List<Card> Cards,
     List<LicensePlate> LicensePlates,
     List<Pin> Pins,
     List<QrCode> QrCodes,
     Face Face
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
            this.RoleGuid = RoleGuid;
            this.CompanyGuid = CompanyGuid;
            this.DepartmentGuid = DepartmentGuid;
            this.PositionGuid = PositionGuid;
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
      }


}

