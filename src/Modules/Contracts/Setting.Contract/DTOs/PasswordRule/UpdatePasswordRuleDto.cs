namespace Setting.Contract.DTOs.PasswordRule;

public sealed record UpdatePasswordRuleDto(
  Guid Guid,
  int Len,
  bool IsDigit,
  bool IsLower,
  bool IsSymbol,
  bool IsUpper,
  List<string> Weaks
);

