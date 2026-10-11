using SharedKernel.Enums;

namespace Aero.Application.Metadata.Door;

public sealed class ForceOpenMaskMetadata
{
      public short DoorNumber { get; set; }
      public MaskAction SetClear { get; set; }
}