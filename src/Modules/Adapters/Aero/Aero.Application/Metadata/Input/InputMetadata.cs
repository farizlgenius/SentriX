namespace Aero.Application.Metadata.Device;

public sealed class InputMetadata
{
      public short Mode { get; set; }
      public short Debounce { get; set; }
      public short HoldTime { get; set; }
      public short LfCode {get; set; }
      public short LatchingMode {get; set; }
      public short DelayEntry {get; set; }
      public short DelayExit {get; set; }

}