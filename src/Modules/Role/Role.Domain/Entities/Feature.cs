using System;
using SharedKernel.Helpers;

namespace Role.Domain.Entities;

public sealed class Feature
{
  public int Id { get; private set; }
  public string Name { get; private set; } = string.Empty;
  public DateTime CreatedAt { get; private set; }
  public DateTime UpdatedAt { get; private set; }

  public Feature(int id, string name, DateTime createdAt, DateTime updatedAt)
  {
    ValidationHelper.ValidateNotMinus(id, nameof(id));
    ValidationHelper.IsNullOrEmpty(name, nameof(name));
    Id = id;
    Name = name;
    CreatedAt = createdAt;
    UpdatedAt = updatedAt;
  }

}
