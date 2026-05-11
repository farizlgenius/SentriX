using System;
using SharedKernel.Helpers;

namespace Location.Domain.Entities;

public sealed class Country
{
  public int Id { get; private set; }
  public string Name { get; private set; } = string.Empty;
  public string Description { get; private set; } = string.Empty;
  public DateTime CreatedAt { get; private set; }
  public DateTime UpdatedAt { get; private set; }

  public Country(int id, string name, string description, DateTime createdAt, DateTime updatedAt)
  {
    ValidationHelper.ValidateNotMinus(id, nameof(id));
    ValidationHelper.ValidateNotNullOrEmpty(name, nameof(name));
    Id = id;
    Name = name;
    Description = description;
    CreatedAt = createdAt;
    UpdatedAt = updatedAt;
  }

}
