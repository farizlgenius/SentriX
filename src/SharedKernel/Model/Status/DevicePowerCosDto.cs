using SharedKernel.Enums;

namespace SharedKernel.Model.Status;

public sealed record DevicePowerCosDto(
      Guid Guid,
      DevicePowerFault Status
);