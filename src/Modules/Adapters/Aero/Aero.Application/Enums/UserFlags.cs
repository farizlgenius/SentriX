namespace Aero.Application.Enums;

 [Flags]
  public enum UserFlag : byte
  {
    Active = 0x01,
    OneFreeApb = 0x02,
    ApbExempt = 0x04,
    ADA = 0x08,
    PinExempt = 0x10,
      NoApb = 0x20,
    NoUseLimit = 0x40,
    NoUseLimitUp = 0x80
  }
