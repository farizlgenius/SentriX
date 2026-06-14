using SharedKernel.Domain;

namespace User.Contract.DTOs;

public sealed record CredentialDto(
      int Id,
      short Flag,
      long CardNumber,
      int IssueCode,
      string Pin,
      short UseCount,
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