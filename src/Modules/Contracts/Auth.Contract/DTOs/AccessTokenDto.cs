using System;

namespace Auth.Contract.DTOs;

public sealed record AccessTokenDto(
      string AccessToken,
      string RefreshToken,
      int ExpiredAt,
      DateTime RefreshExpiredAt
);
