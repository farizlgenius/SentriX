namespace User.Contract.DTOs;

public sealed record PinDto(
      Guid Guid=default,
      string Pin=""
);