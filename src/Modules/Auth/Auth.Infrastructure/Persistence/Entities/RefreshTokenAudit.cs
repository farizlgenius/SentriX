using System;
using SharedKernel.Domain;

namespace Auth.Infrastructure.Persistence.Entities;

public sealed class RefreshTokenAudit : BaseEntity
{
  public string username { get; set; } = string.Empty;
  public string user_id { get; set; } = string.Empty;
  public string hashed_refresh_token { get; set; } = string.Empty;
  public string action { get; set; } = string.Empty;
  public DateTime expired_at { get; set; }

  public RefreshTokenAudit() { }
  

}