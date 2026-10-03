using SharedKernel.Enums;

namespace SharedKernel.Domain;

public sealed record StatusDto(
      Guid Guid,
      Status Status,
      InputStatus Tamper=InputStatus.Unknown,
      InputStatus Ac=InputStatus.Unknown,
      InputStatus Batt=InputStatus.Unknown
);

