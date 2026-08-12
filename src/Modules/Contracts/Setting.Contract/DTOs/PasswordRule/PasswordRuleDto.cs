namespace Setting.Contract.DTOs.PasswordRule;

public sealed record PasswordRuleDto(
  Guid Guid,
  int Len,
  bool IsDigit,
  bool IsLower,
  bool IsSymbol,
  bool IsUpper,
  List<string> Weaks
);

