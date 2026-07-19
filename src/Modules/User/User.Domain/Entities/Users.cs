using System;
using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace User.Domain.Entities;

public sealed class Users : BaseDomainEntity
{
      public string Identification { get; private set; } = string.Empty;
      public string Title { get; private set; } = string.Empty;
      public string FirstName { get; private set; } = string.Empty;
      public string MiddleName { get; private set; } = string.Empty;
      public string LastName { get; private set; } = string.Empty;
      public string Gender { get; private set; } = string.Empty;
      public DateTime DateOfBirth { get; private set; }
      public string Email { get; private set; } = string.Empty;
      public string Phone { get; private set; } = string.Empty;
      public Guid? CompanyGuid { get; private set; }
      public Guid? DepartmentGuid { get; private set; }
      public Guid? PositionGuid { get; private set; }
      public string Address { get; private set; } = string.Empty;
      public DateTime ActiveTime { get; set; }
      public DateTime ExpireTime { get; set; }
      public List<string> Additionals { get; private set; } = new List<string>();
      public List<Guid> Groups { get; private set; } = new List<Guid>();
      public Card Card {get; private set;} = default!;
      public LicensePlate LicensePlate {get; private set;}= default!;
      public Pin Pin {get; private set;}= default!;
      public QrCode QrCode {get; private set;}= default!;
      public Face Face {get; private set;}= default!;

      public Users(
            Guid Guid,
      string UserId,
      string Title,
      string FirstName,
      string MiddleName,
      string LastName,
      string Gender,
      DateTime DateOfBirth,
      string Email,
      string Phone,
      Guid CompanyGuid,
      Guid DepartmentGuid,
      Guid PositionGuid,
      string Address,
      DateTime ActiveTime,
      DateTime ExpireTime,
      List<string> Additionals,
      Card Card,
      LicensePlate LicensePlate,
      Pin Pin,
      QrCode QrCode,
      Face Face,
      List<Guid> UserGroups,
      int locationId, 
      bool IsActive,
      bool IsDefault
      ) : base(Guid,0, locationId, IsActive,IsDefault)
      {
            ValidationHelper.IsValidOnlyCharAndDigit(UserId, nameof(UserId));
            ValidationHelper.IsValidName(FirstName);
            ValidationHelper.IsValidName(LastName);
            // Gender Validate
            ValidationHelper.IsValidEmail(Email,nameof(Email));
            ValidationHelper.ValidateGuid(CompanyGuid,nameof(CompanyGuid));
            ValidationHelper.ValidateGuid(DepartmentGuid,nameof(DepartmentGuid));
            ValidationHelper.ValidateGuid(PositionGuid,nameof(PositionGuid));
            ValidationHelper.ValidateActiveTime(ActiveTime,ExpireTime);
            this.Identification = UserId;
            this.Title = Title;
            this.FirstName = FirstName;
            this.MiddleName = MiddleName;
            this.LastName = LastName;
            this.Gender = Gender;
            this.DateOfBirth = DateOfBirth;
            this.Email = Email;
            this.Phone = Phone;
            this.CompanyGuid = CompanyGuid;
            this.DepartmentGuid = DepartmentGuid;
            this.PositionGuid = PositionGuid;
            this.Address = Address;
            this.ActiveTime = ActiveTime;
            this.ExpireTime = ExpireTime;
            this.Additionals = Additionals;
            this.Card = Card;
            this.LicensePlate = LicensePlate;
            this.Pin = Pin;
            this.QrCode = QrCode;
            this.Face = Face;
            this.Groups = UserGroups;
      }
}
