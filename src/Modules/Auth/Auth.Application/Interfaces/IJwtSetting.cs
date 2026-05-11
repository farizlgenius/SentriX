using System;

namespace Auth.Application.Interfaces;

public interface IJwtSetting
{
      string Secret { get; }
      string Issuer { get; }
      string Audience { get; }
      short AccessTokenMinutes { get; }
      short RefreshTokenDays { get; }
}
