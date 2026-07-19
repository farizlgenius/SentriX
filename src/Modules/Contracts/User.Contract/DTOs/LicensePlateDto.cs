namespace User.Contract.DTOs;

public sealed record LicensePlateDto(
      Guid Guid=default,
      string LicensePlate=""
);