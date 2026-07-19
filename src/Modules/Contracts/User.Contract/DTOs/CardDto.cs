namespace User.Contract.DTOs;

public sealed record CardDto(
      Guid Guid=default,
      short Bits=0,
      int CardNumber=-1
);