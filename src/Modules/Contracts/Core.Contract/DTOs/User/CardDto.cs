namespace Core.Contract.DTOs.User;

public sealed record CardDto(
  short Bits,
  int Fac,
  int CardNumber
);