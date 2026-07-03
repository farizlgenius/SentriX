using SharedKernel.Domain;

namespace User.Contract.DTOs;

public sealed record CredentialDto(
      int Id,
      int Flag,
      short Bits,
      short Fac,
      int CardNumber,
      short IssueCode,
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