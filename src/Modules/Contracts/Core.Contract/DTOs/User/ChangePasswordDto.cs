namespace Core.Contract.DTOs.User;

public sealed record ChangePasswordDto(
  string Username,
  string Old,
  string New
);