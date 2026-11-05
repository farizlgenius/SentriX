using SharedKernel.Enums;

namespace SharedKernel.Model.Status;

public sealed record ModulePowerCosDto(
      Guid Guid,
       DevicePowerFault Status
);