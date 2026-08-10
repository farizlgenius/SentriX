using System;
using SharedKernel.Domain;

namespace Auth.Infrastructure.Persistence.Entities;

public sealed class ApiKey : BaseEntity
{
  public string key { get; set; } = string.Empty;
  public string owner { get; set; } = string.Empty;
  public DateTime expired_at { get; set; }

  public ApiKey() { }

}
