using SharedKernel.Enums;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class User : BaseEntity
{
      public string username { get; set; } = string.Empty;
      public string password { get; set; } = string.Empty;
      public string user_code { get; set; } = string.Empty;
      public string identification { get; set; } = string.Empty;
      public Title title { get; set; } = Title.Mr;
      public string firstname { get; set; } = string.Empty;
      public string middlename { get; set; } = string.Empty;
      public string lastname { get; set; } = string.Empty;
      public Gender gender { get; set; } = Gender.M;
      public DateTime date_of_birth { get; set; }
      public string email { get; set; } = string.Empty;
      public string phone { get; set; } = string.Empty;
      public bool is_operator { get; set; } = false;
      public bool is_user { get; set; } = true;
      public int? role_id { get; set; }
      public Role? role { get; set; } = null!;
      public int? company_id { get; set; }
      public Company? company { get; set; } = null!;
      public int? department_id { get; set; }
      public Department? department { get; set; } = null!;
      public int? position_id { get; set; }
      public Position? position { get; set; } = null!;
      public string address { get; set; } = string.Empty;
      public DateTime active_time { get; set; }
      public DateTime expire_time { get; set; }
      public ICollection<UserAdditional> additionals { get; set; } = default!;
      public ICollection<Card> cards { get; set; } = default!;
      public ICollection<Pin> pins { get; set; } = default!;
      public int? face_id { get; set; }
      public Face? face { get; set; }
      public ICollection<LicensePlate> license_plates { get; set; } = default!;
      public ICollection<QrCode> qr_codes { get; set; } = default!;
      public ICollection<UserLocation> user_locations { get; set; } = default!;
      public ICollection<UserGroup> user_groups { get; set; } = default!;

      public User() { }

      public User(Core.Domain.Entities.User d) : base(d.Guid)
      {
            this.username = d.Username;
            this.identification = d.Identification;
            this.title = d.Title;
            this.firstname = d.FirstName;
            this.middlename = d.MiddleName;
            this.lastname = d.LastName;
            this.gender = d.Gender;
            this.date_of_birth = d.DateOfBirth;
            this.email = d.Email;
            this.phone = d.Phone;
            this.is_operator = d.IsOperator;
            this.role_id = d.RoleId;
            this.company_id = d.CompanyId;
            this.department_id = d.DepartmentId;
            this.position_id = d.PositionId;
            this.address = d.Address;
            this.active_time = d.JoinedTime;
            this.expire_time = d.ExpiredTime;
            this.additionals = d.Additionals.Select(x => new UserAdditional(x)).ToArray();
            this.cards = d.Cards.Select(x => new Card(x)).ToArray();
            this.pins = d.Pins.Select(x => new Pin(x)).ToArray();
            if (d.Face is not null)
                  this.face = new Face(d.Face);
            this.license_plates = d.LicensePlates.Select(x => new LicensePlate(x)).ToArray();
            this.qr_codes = d.QrCodes.Select(x => new QrCode(x)).ToArray();
            this.user_locations = d.LocationIds.Select(x => new UserLocation(

            )).ToArray();
      }

}