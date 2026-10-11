using SharedKernel.Enums;

namespace Aero.Application.Metadata.Door;

public sealed class TempDoorModeMetadata
{
      public short DoorNumber { get; set; }
      public DoorMode Mode { get; set; }
      public short Time {get; set;}
}