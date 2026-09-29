using SharedKernel.Constants;
using SharedKernel.Enums;

namespace Core.Contract.DTOs.Device;

public sealed record DeviceComponentDto(
      bool IsSynced,
      List<Components> Components
);

public sealed record Components(
      EntityType type,
      int total,
      int uploaded,
      int remain,
      bool isSynced
);