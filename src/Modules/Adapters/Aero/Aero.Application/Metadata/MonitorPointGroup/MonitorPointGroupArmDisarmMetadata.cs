using SharedKernel.Enums;

namespace Aero.Application.Metadata.Device;

public sealed class MonitorPointGroupArmDisarmMetadata
{
      public short InputGroupNumber {get; set;}
      public InputGroupCommand Command { get; set; }
      public short Arg1 { get; set; }

}

