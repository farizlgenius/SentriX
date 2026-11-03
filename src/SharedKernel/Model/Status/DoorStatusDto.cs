using SharedKernel.Enums;

namespace SharedKernel.Model.Status;

public sealed record DoorStatusDto(
      Guid Guid,
      DoorMode Mode,
      ReaderStatus ReaderStatus,
      ChangeOfState StrikeStatus,
      ChangeOfState SensorStatus,
      ChangeOfState DoorStatus,
      ChangeOfState Rex0Status,
      ChangeOfState Rex1Status,
      short LedModeControl,
      short AccessControlFlag,
      ReaderStatus AlternateReaderStatus,
      short ExtendedAccessControlFlag,
      short ExtendedFeatureType,
      short DeviceType,
      string ExtendedFeatureStatus 
);