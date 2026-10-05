namespace Aero.Application.Enums;

  [Flags]
  public enum AccessPointStatus : byte
  {
    None = 0x00,
    Unlocked = 0x01,
    ExitCycleInProgress = 0x02,
    ForcedOpen = 0x04,
    ForcedOpenMasked = 0x08,
    HeldOpen = 0x10,
    HeldOpenMasked = 0x20,
    HeldOpenPreAlarm = 0x40,
    ExtendedHeldOpenMode = 0x80
  }