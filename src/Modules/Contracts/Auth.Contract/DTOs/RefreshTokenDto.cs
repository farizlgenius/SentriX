namespace Auth.Contract.DTOs;

public sealed record RefreshTokenDto(
      string HashedToken,
      string Username,
      string Action,
      DateTime ExpiredAt
);