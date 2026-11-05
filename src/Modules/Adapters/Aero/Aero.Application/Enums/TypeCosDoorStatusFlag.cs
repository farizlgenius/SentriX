namespace Aero.Application.Enums;

 [Flags]
  public enum TypeCosDoorStatusFlag : byte
  {
    UNLOCKED = 0x01,
    EXIT_CYCLE = 0x02,
    FORCED_OPEN = 0x04,
    FORCED_OPEN_MASKED = 0x08,
    HELD_OPEN = 0x10,
    HELD_OPEN_MASKED = 0x20,
    HELD_PRE_ALARM = 0X40,
 EXTENDED_HELD_MODE = 0X80
  }
