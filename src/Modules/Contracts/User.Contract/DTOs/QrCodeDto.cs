namespace User.Contract.DTOs;

public sealed record QrCodeDto(
      Guid Guid=default,
      string QrCode=""
);