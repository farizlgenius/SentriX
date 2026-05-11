using System;
using SharedKernel.Domain;

namespace Operator.Infrastructure.Persistences.Entities;


public sealed class Operators : BaseEntity
{
  public string username { get; set; } = string.Empty;
  public string password { get; set; } = string.Empty;
  public string title { get; set; } = string.Empty;
  public string firstname { get; set; } = string.Empty;
  public string middlename { get; set; } = string.Empty;
  public string lastname { get; set; } = string.Empty;
  public string gender { get; set; } = string.Empty;
  public string email { get; set; } = string.Empty;
  public string mobile { get; set; } = string.Empty;
  public int role_id { get; set; }
  public ICollection<OperatorLocation> operator_locations { get; set; } = new List<OperatorLocation>();

  public Operators() { }
  public Operators(Operator.Domain.Entities.Operators domain)
  {
    username = domain.Username;
    title = domain.Title;
    firstname = domain.FirstName;
    middlename = domain.MiddleName;
    lastname = domain.LastName;
    gender = domain.Gender;
    email = domain.Email;
    mobile = domain.Mobile;
    role_id = domain.RoleId;
    created_at = DateTime.UtcNow;
    updated_at = DateTime.UtcNow;
  }

  public void AddPassword(string password)
  {
    this.password = password;
  }


  public void UpdatePassword(string hashed)
  {
    this.password = hashed;
  }

  public void Update(Operator.Domain.Entities.Operators user)
  {
    username = user.Username;
    title = user.Title;
    firstname = user.FirstName;
    middlename = user.MiddleName;
    lastname = user.LastName;
    gender = user.Gender;
    email = user.Email;
    mobile = user.Mobile;
    role_id = user.RoleId;
    updated_at = DateTime.UtcNow;
  }
}
