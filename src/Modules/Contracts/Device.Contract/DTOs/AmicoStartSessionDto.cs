namespace Device.Contract.DTOs;

public sealed record AmicoStartSessionDto(
      string Login,
      string Password,
      string Ip
);