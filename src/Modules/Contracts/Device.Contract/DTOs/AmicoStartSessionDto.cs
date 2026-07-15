namespace Device.Contract.DTOs;

public sealed record AmicoStartSessionDto(
      string Ip,
      bool IsFirst
);