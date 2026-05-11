using System;
using Auth.Application.Interfaces;

namespace Auth.Application.ValueObject;

public class JwtSetting : IJwtSetting
{
      public string Secret { get; set; } = string.Empty;

  public string Issuer { get; set; } = string.Empty;

  public string Audience { get; set; } = string.Empty;

  public short AccessTokenMinutes { get; set; }
  public short RefreshTokenDays { get; set; }
}
