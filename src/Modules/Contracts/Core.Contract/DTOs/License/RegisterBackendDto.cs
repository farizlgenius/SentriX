namespace Core.Contract.DTOs.License;

public sealed record RegisterServerDto(
  Guid ProvisionGuid,
  string ProvisionToken
);