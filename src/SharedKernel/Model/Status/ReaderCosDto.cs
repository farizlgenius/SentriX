using SharedKernel.Enums;

namespace SharedKernel.Model.Status;


public sealed record ReaderCosDto(
      Guid Guid,
      ReaderStatus Status
);

