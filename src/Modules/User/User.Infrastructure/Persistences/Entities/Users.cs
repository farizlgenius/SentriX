using System;
using System.ComponentModel.DataAnnotations;
using SharedKernel.Domain;

namespace User.Infrastructure.Persistences.Entities;

public sealed class Users : BaseDbEntity
{
  [Required]
  public string identification { get; set; } = string.Empty;
  public string title { get; set; } = string.Empty;
  public string first_name { get; set; } = string.Empty;
  public string middle_name { get; set; } = string.Empty;
  public string last_name { get; set; } = string.Empty;
  public string gender { get; set; } = string.Empty;
  public DateTime date_of_birth { get; set; }
  public string email { get; set; } = string.Empty;
  public string phone { get; set; } = string.Empty;
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
  public Guid? card_guid { get; set; }
  public Card? card { get; set; }
  public Guid? pin_guid { get; set; }
  public Pin? pin { get; set; }
  public Guid? face_guid { get; set; }
  public Face? face { get; set; }
  public Guid? license_plate_guid { get; set; }
  public LicensePlate? license_plate { get; set; }
  public Guid? qr_code_guid { get; set; }
  public QrCode? qr_code { get; set; }

  public ICollection<UserGroup> user_groups { get; set; } = default!;
  public Users() { }
  public Users(Domain.Entities.Users users) : base(users.Guid, 0, users.LocationId, users.IsActive, false)
  {
    this.identification = users.Identification;
    this.title = users.Title;
    this.first_name = users.FirstName;
    this.middle_name = users.MiddleName;
    this.last_name = users.LastName;
    this.gender = users.Gender;
    this.date_of_birth = users.DateOfBirth;
    this.email = users.Email;
    this.phone = users.Phone;
    this.company_guid = users.CompanyGuid;
    this.department_guid = users.DepartmentGuid;
    this.position_guid = users.PositionGuid;
    this.address = users.Address;
    this.active_time = users.ActiveTime;
    this.expire_time = users.ExpireTime;
    this.card = new Card(users.Card);
    this.pin = new Pin(users.Pin);
    this.face = new Face(users.Face);
    this.license_plate = new LicensePlate(users.LicensePlate);
    this.qr_code = new QrCode(users.QrCode);
  }

  public void Update(Domain.Entities.Users users)
  {
    this.title = users.Title;
    this.first_name = users.FirstName;
    this.middle_name = users.MiddleName;
    this.last_name = users.LastName;
    this.gender = users.Gender;
    this.date_of_birth = users.DateOfBirth;
    this.email = users.Email;
    this.phone = users.Phone;
    this.company_guid = users.CompanyGuid;
    this.department_guid = users.DepartmentGuid;
    this.position_guid = users.PositionGuid;
    this.address = users.Address;
    this.updated_at = DateTime.UtcNow;
    this.active_time = users.ActiveTime;
    this.expire_time = users.ExpireTime;

    if (this.card is not null)
    {
      this.card.Update(users.Card);
    }
    else
    {
      this.card = new Card(users.Card);
    }

    if (this.pin is not null)
    {
      this.pin.Update(users.Pin);
    }
    else
    {
      this.pin = new Pin(users.Pin);
    }

    if (this.face is not null)
    {
      this.face.Update(users.Face);
    }
    else
    {
      this.face = new Face(users.Face);
    }

    if (this.license_plate is not null)
    {
      this.license_plate.Update(users.LicensePlate);
    }
    else
    {
      this.license_plate = new LicensePlate(users.LicensePlate);
    }

    if (this.qr_code is not null)
    {
      this.qr_code.Update(users.QrCode);
    }
    else
    {
      this.qr_code = new QrCode(users.QrCode);
    }



  }


}
