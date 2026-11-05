using SharedKernel.Enums;

namespace SharedKernel.Model.Status;

public sealed record DoorCosDto(
      Guid Guid,
      DoorStatus Status,
      bool Unlocked,
      bool ExitCycle,
      bool ForcedOpen,
      bool ForcedOpenMask,
      bool HeldOpen,
      bool HeldOpenMask,
      bool PreAlarm,
      bool ExtendedHeldOpen
);