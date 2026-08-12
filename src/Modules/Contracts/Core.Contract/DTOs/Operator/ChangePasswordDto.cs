namespace Core.Contract.DTOs.Operator;

public sealed record ChangePasswordDto(
  string Username,
  string Old,
  string New
);