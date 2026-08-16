namespace Core.Contract.DTOs.License;

public sealed record RegisterBackendDto(
  string ProvisionToken,
  string PublicKey,
  string BackendName
);