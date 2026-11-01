using SharedKernel.Enums;

namespace SharedKernel.Model.Status;

public sealed record OutputStatusDto(
      Guid Guid,
      ChangeOfState Status,
      bool Offline,
      bool Mask,
      bool LocalMask,
      bool EntryDelayInProgress,
      bool NotAttached
);

