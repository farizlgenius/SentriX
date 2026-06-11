using SharedKernel.Domain;

namespace User.Contract.DTOs;

public sealed record CredentialDto(
      int Id,
      string UserId,
      string Password,
      string Salt,
      short Flag
) : BaseDto(
      0,
      0,
      string.Empty,
      true
);