using System;
using System.ComponentModel.DataAnnotations;
using SharedKernel.Domain;

namespace User.Infrastructure.Persistences.Entities;

public sealed class Users : BaseEntity
{
  [Required]
  public string user_id { get; set; } = string.Empty;
  public string title { get; set; } = string.Empty;
  public string first_name { get; set; } = string.Empty;
  public string middle_name { get; set; } = string.Empty;
  public string last_name { get; set; } = string.Empty;
  public string gender { get; set; } = string.Empty;
  public DateTime date_of_birth { get; set; }
  public string email { get; set; } = string.Empty;
  public string phone { get; set; } = string.Empty;
  public int company_id { get; set; }
  public Company company { get; set; } = new Company();
  public int department_id { get; set; }
  public Department department { get; set; } = new Department();
  public int position_id { get; set; }
  public Position position { get; set; } = new Position();
  public string address { get; set; } = string.Empty;
  public ICollection<UserAdditional> additionals { get; set; } = new List<UserAdditional>();
  public string image { get; set; } = string.Empty;
  public ICollection<Credential> credentials { get; set; } = new List<Credential>();
  public ICollection<UserGroup> user_groups { get; set; } = new List<UserGroup>();
  public int vacation_id {get; set;}
  public Vacation vacation {get; set;} = default!;
  public Users() { }
  public Users(Domain.Entities.Users users) : base(0,users.LocationId,users.IsActive)
  {
      this.user_id = users.UserId;
      this.title = users.Title;
      this.first_name = users.FirstName;
      this.middle_name = users.MiddleName;
      this.last_name = users.LastName;
      this.gender = users.Gender;
      this.date_of_birth = users.DateOfBirth;
      this.email = users.Email;
      this.phone = users.Phone;
      this.company_id = users.CompanyId;
      this.department_id = users.DepartmentId;
      this.position_id = users.PositionId;
      this.address = users.Address;
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
      this.company_id = users.CompanyId;
      this.department_id = users.DepartmentId;
      this.position_id = users.PositionId;
      this.address = users.Address;
      this.updated_at = DateTime.UtcNow;
  }


}
