namespace Core.Infrastructure.Persistences.Entities;

public sealed class User : BaseEntity
{
      public string username { get; set; } = string.Empty;
      public string password { get; set; } = string.Empty;
      public string identification {get; set;} =string.Empty;
      public string title { get; set; } = string.Empty;
      public string firstname { get; set; } = string.Empty;
      public string middlename { get; set; } = string.Empty;
      public string lastname { get; set; } = string.Empty;
      public string gender { get; set; } = string.Empty;
      public DateTime date_of_birth { get; set; }
      public string email { get; set; } = string.Empty;
      public string phone { get; set; } = string.Empty;
      public bool is_operator {get; set;}
      public Guid? role_guid { get; set; }
      public Role? role { get; set; }
      public Guid? company_guid { get; set; }
      public Company? company { get; set; }
      public Guid? department_guid { get; set; }
      public Department? department { get; set; }
      public Guid? position_guid { get; set; }
      public Position? position { get; set; } = default!;
      public string address { get; set; } = string.Empty;
      public DateTime active_time { get; set; }
      public DateTime expire_time { get; set; }
      public ICollection<UserAdditional> additionals { get; set; } = default!;
      public ICollection<Card> cards {get; set;} = default!;
      public ICollection<Pin> pins {get; set;} = default!;
      public Guid? face_guid { get; set; }
      public Face? face { get; set; }
      public ICollection<LicensePlate> license_plates {get; set;} = default!;
      public ICollection<QrCode> qr_codes {get; set;} = default!;

      public User(){}

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
            this.role_guid = d.RoleGuid;
            this.company_guid = d.CompanyGuid;
            this.department_guid = d.DepartmentGuid;
            this.position_guid = d.PositionGuid;
            this.address = d.Address;
            this.active_time = d.ActiveTime;
            this.expire_time = d.ExpireTime;
            this.additionals = d.Additionals.Select(x => new UserAdditional(x)).ToArray();
            this.cards = d.Cards.Select(x => new Card(x)).ToArray();
            this.pins = d.Pins.Select(x => new Pin(x)).ToArray();
            if(d.Face is not null)
                  this.face = new Face(d.Face);
            this.license_plates = d.LicensePlates.Select(x => new LicensePlate(x)).ToArray();
            this.qr_codes = d.QrCodes.Select(x => new QrCode(x)).ToArray();
      }

}