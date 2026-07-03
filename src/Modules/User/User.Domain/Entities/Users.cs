using System;
using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace User.Domain.Entities;

public sealed class Users : BaseDomain
{
      public string UserId { get; set; } = string.Empty;
      public string Title { get; set; } = string.Empty;
      public string FirstName { get; set; } = string.Empty;
      public string MiddleName { get; set; } = string.Empty;
      public string LastName { get; set; } = string.Empty;
      public string Gender { get; set; } = string.Empty;
      public DateTime DateOfBirth { get; set; }
      public string Email { get; set; } = string.Empty;
      public string Phone { get; set; } = string.Empty;
      public int CompanyId { get; set; }
      public int DepartmentId { get; set; }
      public int PositionId { get; set; }
      public string Address { get; set; } = string.Empty;
      public int Flag {get; set;} 
      public List<string> Additionals { get; set; } = new List<string>();
      public string Image { get; set; } = string.Empty;
      public List<Credential> Credentials { get; set; } = new List<Credential>();
      public List<int> Groups { get; set; } = new List<int>();

      public Users(int id,
      string UserId,
      string Title,
      string FirstName,
      string MiddleName,
      string LastName,
      string Gender,
      DateTime DateOfBirth,
      string Email,
      string Phone,
      int CompanyId,
      int DepartmentId,
      int PositionId,
      string Address,
      int Flag,
      List<string> Additionals,
      string Image,
      List<Credential> Credentials,
      List<int> UserGroups,
      int locationId, 
      bool IsActive
      ) : base(id,0, locationId, IsActive)
      {
            ValidationHelper.IsValidOnlyCharAndDigit(UserId, nameof(UserId));
            ValidationHelper.IsValidName(FirstName);
            ValidationHelper.IsValidName(LastName);
            // Gender Validate
            ValidationHelper.IsValidEmail(Email,nameof(Email));
            ValidationHelper.ValidateNotMinus(CompanyId,nameof(CompanyId));
            ValidationHelper.ValidateNotMinus(DepartmentId,nameof(DepartmentId));
            ValidationHelper.ValidateNotMinus(PositionId,nameof(PositionId));
            this.UserId = UserId;
            this.Title = Title;
            this.FirstName = FirstName;
            this.MiddleName = MiddleName;
            this.LastName = LastName;
            this.Gender = Gender;
            this.DateOfBirth = DateOfBirth;
            this.Email = Email;
            this.Phone = Phone;
            this.CompanyId = CompanyId;
            this.DepartmentId = DepartmentId;
            this.PositionId = PositionId;
            this.Address = Address;
            this.Flag = Flag;
            this.Additionals = Additionals;
            this.Image = Image;
            this.Credentials = Credentials;
            this.Groups = UserGroups;
      }
}
