using SharedKernel.Enums;

namespace SharedKernel.Model.Status;

public sealed record ModuleStatusDto(
      Guid Guid,
       ModuleCommStatus Status,
       ModuleEncConfig EncConfig,
       ModuleEncStatus EncStatus
);