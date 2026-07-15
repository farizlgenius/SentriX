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
      public Guid Guid {get; set;}
      public Guid ReaderModuleGuid { get; set; }
      public short ReaderNumber { get; set; }
}

public sealed class ReaderOut
{
      public Guid Guid {get; set;}
      public Guid ReaderModuleGuid { get; set; }
      public short ReaderNumber { get; set; }
}

public sealed class Sensor
{
      public Guid Guid {get; set;}
      public Guid SensorModuleGuid { get; set; }
      public short SensorNumber { get; set; }
}

public sealed class Relay
{
      public Guid Guid {get; set;}
      public Guid RelayModuleGuid { get; set; }
      public short RelayNumber { get; set; }
}

public sealed class Rex
{
      public Guid Guid {get; set;}
      public Guid Rex0ModuleGuid { get; set; }
      public short Rex0Number { get; set; }
      public Guid Rex1ModuleGuid { get; set; }
      public short Rex1Number { get; set; }
}

public sealed class AltrReader
{
      public Guid Guid {get; set;}
      public Guid AltrRdrModuleId { get; set; }
      public short AltrRdrNumber { get; set; }
}
