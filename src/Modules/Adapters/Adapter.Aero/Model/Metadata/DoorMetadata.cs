namespace Adapter.Aero.Model.Metadata;

public sealed class DoorMetadata
{
      public short AccessConfig { get; set; }
      public ReaderIn ReaderIn { get; set; } = new ReaderIn();
      public ReaderOut ReaderOut { get; set; } = new ReaderOut();
      public Sensor Sensor {get; set;} = new Sensor(); 
      public Relay Relay { get; set; } = new Relay();
      public Rex Rex { get; set; } = new Rex();
      public AltrReader AltrReader { get; set; } = new AltrReader();
      public Antipassback Antipassback { get; set; } = new Antipassback();
      public short Spare { get; set; }
      public short AccessControlFlag { get; set; }
      public short OfflineMode { get; set; }
      public short DefaultMode { get; set; }
      public short LedMode { get; set; }
      public short ApbDelay { get; set; }
      public short RelayT2 { get; set; }
      public short HeldOpen2 { get; set; }
      public short RelayFollowerPulse { get; set; }
      public short RelayFollowerDelay { get; set; }
      public short ExtendFeatureType { get; set; }
      public short InteriorPushButtonModuleComponentId { get; set; }
      public short InteriorPushButtonInputNumber { get; set; }
      public short InteriorPushButtonLongPress { get; set; }
      public short InteriorPushButtonOutModuleComponentId { get; set; }
      public short InteriorPushButtonOutRelayNumber { get; set; }
}

public sealed class ReaderIn
{
      public short ReaderModuleComponentId { get; set; }
      public short ReaderNumber { get; set; }
      public short DataFormat { get; set; } = 0x01;
      public short KeypadMode { get; set; } = 2;
      public short LedDriveMode { get; set; }
      public bool OsdpFlag { get; set; }
      public short OsdpBaudrate { get; set; } = 0x01;
      public short OsdpDiscover { get; set; } = 0x08;
      public short OsdpTracing { get; set; } = 0x10;
      public short OsdpAddress { get; set; }
      public short OsdpSecureChannel { get; set; }
}

public sealed class ReaderOut
{
      public short ReaderModuleComponentId { get; set; }
      public short ReaderNumber { get; set; }
      public short DataFormat { get; set; } = 0x01;
      public short KeypadMode { get; set; } = 2;
      public short LedDriveMode { get; set; }
      public bool OsdpFlag { get; set; }
      public short OsdpBaudrate { get; set; } = 0x01;
      public short OsdpDiscover { get; set; } = 0x08;
      public short OsdpTracing { get; set; } = 0x10;
      public short OsdpAddress { get; set; }
      public short OsdpSecureChannel { get; set; }
}

public sealed class Relay
{
      public short RelayModuleComponentId { get; set; }
      public short RelayNumber { get; set; }
      public short RelayMin { get; set; }
      public short RelayMax { get; set; }
      public short RelayMode { get; set; }
}

public sealed class Sensor
{
      public short SensorModuleComponentId { get; set; }
      public short SensorNumber { get; set; }
      public short HeldOpenDelay { get; set; }
      public short SensorMode {get; set;}
      public short Debounce {get; set;}
      public short HoldTime {get; set;}
}

public sealed class Rex
{
      public short Rex0ModuleComponentId { get; set; }
      public short Rex0Number { get; set; }
      public short Rex1ModuleComponentId { get; set; }
      public short Rex1Number { get; set; }
      public short DisableRex0Timezone { get; set; }
      public short DisableRex1Timezone { get; set; }
      public short Rex0SensorMode {get; set;}
      public short Rex0Debounce {get; set;}
      public short Rex0HoldTime {get; set;}
      public short Rex1SensorMode {get; set;}
      public short Rex1Debounce {get; set;}
      public short Rex1HoldTime {get; set;}
}

public sealed class AltrReader
{
      public short AltrRdrModuleComponentId { get; set; }
      public short AltrRdrNumber { get; set; }
      public short AltrRdrConf { get; set; }
}

public sealed class Antipassback
{
      public short AntipassbackMode { get; set; }
      public short AreaIn { get; set; }
      public short AreaOut { get; set; }
}