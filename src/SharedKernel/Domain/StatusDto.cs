using SharedKernel.Enums;

namespace SharedKernel.Domain;

public sealed record StatusDto(
      Guid Guid,
      Status Status,
      Status Tamper=Status.Unknown,
      Status Ac=Status.Unknown,
      Status Batt=Status.Unknown
);

