using System;
using SharedKernel.Helpers;

namespace Role.Domain.Entities;

public sealed class Permission
{
  public int Id { get; private set; }
  public string Name { get; private set; } = string.Empty;
  public bool IsEnabled { get; private set; }
  public bool IsCreated { get; private set; }
  public bool IsUpdated { get; private set; }
  public bool IsDeleted { get; private set; }

  public Permission(int id, string name, bool isEnabled, bool isCreated, bool isUpdated, bool isDeleted)
  {
    ValidationHelper.ValidateNotMinus(id, nameof(id));
    ValidationHelper.ValidateNotNullOrEmpty(name, nameof(name));
    Id = id;
    Name = name;
    IsEnabled = isEnabled;
    IsCreated = isCreated;
    IsUpdated = isUpdated;
    IsDeleted = isDeleted;
  }
}
