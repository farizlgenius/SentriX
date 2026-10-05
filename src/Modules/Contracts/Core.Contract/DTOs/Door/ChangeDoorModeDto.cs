using SharedKernel.Enums;

namespace Core.Contract.DTOs.Door;

public sealed record ChangeDoorModeDto(
      Guid Guid,
      DoorMode Mode
);