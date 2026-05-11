using System;
using SharedKernel.Helpers;

namespace Operator.Domain.Entities;

public sealed class Operators
{
  public int Id { get; private set; }
  public string Username { get; private set; } = string.Empty;
  public string Password { get; private set; } = string.Empty;
  public string Title { get; private set; } = string.Empty;
  public string FirstName { get; private set; } = string.Empty;
  public string MiddleName { get; private set; } = string.Empty;
  public string LastName { get; private set; } = string.Empty;
  public string Gender { get; private set; } = string.Empty;
  public string Email { get; private set; } = string.Empty;
  public string Mobile { get; private set; } = string.Empty;
  public int RoleId { get; private set; }
  public List<int> LocationId { get; private set; } = new List<int>();

  public Operators() { }

  public Operators(string username, string password, string title, string firstName, string middleName, string lastName, string gender, string email, string mobile, List<int> locationId, int roleId)
  {
    ValidationHelper.ValidateNotNullOrEmpty(username, nameof(username));
    ValidationHelper.ValidateNotNullOrEmpty(password, nameof(password));
    ValidationHelper.ValidateNotNullOrEmpty(firstName, nameof(firstName));
    ValidationHelper.ValidateNotNullOrEmpty(lastName, nameof(lastName));
    ValidationHelper.ValidateNotNullOrEmpty(email, nameof(email));
    this.Mobile = mobile;
    Username = username;
    Password = password;
    Title = title;
    FirstName = firstName;
    MiddleName = middleName;
    LastName = lastName;
    Gender = gender;
    Email = email;
    Mobile = mobile;
    LocationId = locationId;
    RoleId = roleId;
  }

  public Operators(int id, string username, string title, string firstName, string middleName, string lastName, string gender, string email, string mobile, List<int> locationId, int roleId)
  {
    ValidationHelper.ValidateNotMinus(id, nameof(Id));
    ValidationHelper.ValidateNotNullOrEmpty(username, nameof(username));
    ValidationHelper.ValidateNotNullOrEmpty(firstName, nameof(firstName));
    ValidationHelper.ValidateNotNullOrEmpty(lastName, nameof(lastName));
    ValidationHelper.ValidateNotNullOrEmpty(email, nameof(email));
    Id = id;
    this.Mobile = mobile;
    Username = username;
    Title = title;
    FirstName = firstName;
    MiddleName = middleName;
    LastName = lastName;
    Gender = gender;
    Email = email;
    Mobile = mobile;
    LocationId = locationId;
    RoleId = roleId;
  }

}
