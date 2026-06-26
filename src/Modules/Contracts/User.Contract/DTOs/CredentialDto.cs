using SharedKernel.Domain;

namespace User.Contract.DTOs;

public sealed record CredentialDto(
      int Id,
      short Flag,
      short Bits,
      short Fac,
      long CardNumber,
      int IssueCode,
      string Pin,
      short UseCount,
      short ApbLoc,
      DateTime Active,
      DateTime Expire,
      int LocationId,
      bool IsActive
) : BaseDto(
      0,
      LocationId,
      string.Empty,
      IsActive
);