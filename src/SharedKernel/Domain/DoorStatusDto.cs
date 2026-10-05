using SharedKernel.Enums;

namespace SharedKernel.Domain;

public sealed record DoorStatusDto(
      Guid Guid,
      DoorStatus Status = DoorStatus.Unknown,
      DoorMode Altr1=DoorMode.Unknown,
      ReaderStatus Altr2=ReaderStatus.Unknown,
      InputStatus Altr3=InputStatus.Unknown,
      InputStatus Altr4=InputStatus.Unknown
);

