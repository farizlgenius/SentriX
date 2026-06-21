using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace Door.Domain.Entities;

public sealed class DoorMetadata
{
      public ReaderIn ReaderIn { get; set; } = default!;
      public ReaderOut ReaderOut {get; set;} = default!;
      public Sensor Sensor {get; set;} = default!;
      public Relay Relay {get; set; } = default!;
      public Rex Rex {get; set;} = default!;
      public AltrReader AltrReader {get; set; } = default!;
}

public sealed class ReaderIn
{
      public int ReaderModuleId { get; set; }
      public short ReaderNumber { get; set; }
}

public sealed class ReaderOut
{
      public int ReaderModuleId { get; set; }
      public short ReaderNumber { get; set; }
}

public sealed class Sensor
{
      public int SensorModuleId { get; set; }
      public short SensorNumber { get; set; }
}

public sealed class Relay
{
      public int RelayModuleId { get; set; }
      public short RelayNumber { get; set; }
}

public sealed class Rex
{
      public int Rex0ModuleId { get; set; }
      public short Rex0Number { get; set; }
      public int Rex1ModuleId { get; set; }
      public short Rex1Number { get; set; }
}

public sealed class AltrReader
{
      public int AltrRdrModuleId { get; set; }
      public short AltrRdrNumber { get; set; }
}
