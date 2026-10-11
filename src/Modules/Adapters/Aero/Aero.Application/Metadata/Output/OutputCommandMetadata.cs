namespace Aero.Application.Metadata.Device;

public sealed class OutputCommandMetadata
{
      public short OutputNumber {get; set;}
      public short Command { get; set; }
      public short OnTime { get; set; }
      public short OffTime { get; set; }
      public short Repeat { get; set; }

}