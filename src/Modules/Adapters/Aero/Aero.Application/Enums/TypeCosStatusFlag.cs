namespace Aero.Application.Enums;

 [Flags]
  public enum TypeCosStatusFlag : byte
  {
    None = 0x00,
    Offline = 0x08,
    Masked = 0x10,
    LocalMask = 0x20,
      DelayInProgress = 0x40,
    NotAttached = 0x80
  }
