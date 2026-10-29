using SharedKernel.Enums;

namespace SharedKernel.Domain;

// public sealed record StatusDto(
//       Guid Guid,
//       Status Status,
//       InputStatus Altr1=InputStatus.Unknown,
//       InputStatus Altr2=InputStatus.Unknown,
//       InputStatus Altr3=InputStatus.Unknown,
//       InputStatus Altr4=InputStatus.Unknown
// );

public sealed record StatusDto(
      Guid Guid,
      Status Status,
      DoorStatus DoorStatus=DoorStatus.Unknown,
      InputStatus InputStatus=InputStatus.Unknown,
      InputStatus OutputStatus=InputStatus.Unknown,
      TamperStatus TamperStatus=TamperStatus.Unknown,
      DoorMode DoorMode=DoorMode.Unknown,
      ReaderMode ReaderMode=ReaderMode.Osdp,
      StrikeMode StrikeMode=StrikeMode.Unknown,
      InputStatus CabinetTamper=InputStatus.Unknown,
      InputStatus PowerMonitor=InputStatus.Unknown
);


