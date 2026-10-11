using SharedKernel.Enums;

namespace Aero.Application.Metadata.Door;

public sealed class MomentaryUnlockMetadata
{
      public short DoorNumber { get; set; }
      public short FloorNumber { get; set; }
      public short StrikeTime { get; set; }
      public short HeldOpenTime { get; set; }
      public short HeldOpenPreAlarm { get; set; }
}