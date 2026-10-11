using SharedKernel.Enums;

namespace Aero.Application.Metadata.Door;

public sealed class HeldOpenMaskMetadata
{
      public short DoorNumber { get; set; }
      public MaskAction SetClear { get; set; }
}