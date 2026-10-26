using SharedKernel.Enums;

namespace Aero.Application.Metadata.Area;

public sealed class AreaConfigurationMetadata
{
      public short AreaNumber { get; set; }
      public AreaCommand Command { get; set; }
      public short OccSet { get; set; }
}