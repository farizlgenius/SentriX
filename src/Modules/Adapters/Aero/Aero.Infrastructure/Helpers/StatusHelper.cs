using Aero.Application.Enums;
using Aero.Domain.Entities;
using HID.Aero.ScpdNet.Wrapper;
using SharedKernel.Enums;
using SharedKernel.Model.Status;

namespace Aero.Infrastructure.Helpers;

public static class StatusHelper
{

      public static OutputStatusDto ParseOutputStatus(Guid guid,short s)
      {
            return new OutputStatusDto(
                  guid,
                  StatusTypeCoSDecode(s),
                  ((TypeCosStatusFlag)(s & 0xF8)).HasFlag(TypeCosStatusFlag.Offline),
                  ((TypeCosStatusFlag)(s & 0xF8)).HasFlag(TypeCosStatusFlag.Masked),
                  ((TypeCosStatusFlag)(s & 0xF8)).HasFlag(TypeCosStatusFlag.LocalMask),
                  ((TypeCosStatusFlag)(s & 0xF8)).HasFlag(TypeCosStatusFlag.DelayInProgress),
                  ((TypeCosStatusFlag)(s & 0xF8)).HasFlag(TypeCosStatusFlag.NotAttached)
            );
      }
      public static AreaCosDto ParseAreaChangeOfStateStatus(Guid guid,short d,int occ)
      {
            return new AreaCosDto(
                  guid,
                  ((AreaStatusFlag)d).HasFlag(AreaStatusFlag.ENABLED),
                  ((AreaStatusFlag)d).HasFlag(AreaStatusFlag.MULTI_OCC),
                  ((AreaStatusFlag)d).HasFlag(AreaStatusFlag.NOT_CONFIG),
                  occ
            );
      }

      public static RexCosDto ParseRexChangeOfStateStatus(
            Guid guid,short s
      )
      {
            return new RexCosDto(
                  guid,
                  StatusTypeCoSDecode(s),
                  ((TypeCosStatusFlag)(s & 0xF8)).HasFlag(TypeCosStatusFlag.Offline),
                  ((TypeCosStatusFlag)(s & 0xF8)).HasFlag(TypeCosStatusFlag.Masked),
                  ((TypeCosStatusFlag)(s & 0xF8)).HasFlag(TypeCosStatusFlag.LocalMask),
                  ((TypeCosStatusFlag)(s & 0xF8)).HasFlag(TypeCosStatusFlag.DelayInProgress),
                  ((TypeCosStatusFlag)(s & 0xF8)).HasFlag(TypeCosStatusFlag.NotAttached)
            );
      }

      public static DoorCosDto ParseDoorChangeOfStateStatus(Guid guid,short c,short d)
      {
            return new DoorCosDto(
                  guid,
                  (DoorStatus)c,
                  ((TypeCosDoorStatusFlag)d).HasFlag(TypeCosDoorStatusFlag.UNLOCKED),
                  ((TypeCosDoorStatusFlag)d).HasFlag(TypeCosDoorStatusFlag.EXIT_CYCLE),
                  ((TypeCosDoorStatusFlag)d).HasFlag(TypeCosDoorStatusFlag.FORCED_OPEN),
                  ((TypeCosDoorStatusFlag)d).HasFlag(TypeCosDoorStatusFlag.FORCED_OPEN_MASKED),
                  ((TypeCosDoorStatusFlag)d).HasFlag(TypeCosDoorStatusFlag.HELD_OPEN),
                  ((TypeCosDoorStatusFlag)d).HasFlag(TypeCosDoorStatusFlag.HELD_OPEN_MASKED),
                  ((TypeCosDoorStatusFlag)d).HasFlag(TypeCosDoorStatusFlag.HELD_PRE_ALARM),
                  ((TypeCosDoorStatusFlag)d).HasFlag(TypeCosDoorStatusFlag.EXTENDED_HELD_MODE)
            );
      }

      public static ReaderCosDto ParseReaderStatus(Guid guid,short s)
      {
            return new ReaderCosDto(
                  guid,
                  (ReaderStatus)s
            );
      }
      public static DevicePowerCosDto ParseDevicePowerStatus(Guid guid,short s)
      {
            byte codeValue = (byte)(s & 0x07);
            return new DevicePowerCosDto(
                  guid,
                  (DevicePowerFault)codeValue
            );
      }
      public static ModuleStatusDto ParseModuleCommStatus(Guid guid,ReplyMessage.TypeSioComm c)
      {
            return new ModuleStatusDto(
                  guid,
                  (ModuleCommStatus)c.comm_sts,
                  (ModuleEncConfig)c.nEncConfig,
                  (ModuleEncStatus)c.nEncKeyStatus
            );
      }
      public static CabinetTamperCosDto ParseModuleTamperStatus(Guid guid,short s)
      {
            return new CabinetTamperCosDto(
                  guid,
                  StatusTypeCoSDecode(s),
                  ((TypeCosStatusFlag)(s & 0xF8)).HasFlag(TypeCosStatusFlag.Offline),
                  ((TypeCosStatusFlag)(s & 0xF8)).HasFlag(TypeCosStatusFlag.Masked),
                  ((TypeCosStatusFlag)(s & 0xF8)).HasFlag(TypeCosStatusFlag.LocalMask),
                  ((TypeCosStatusFlag)(s & 0xF8)).HasFlag(TypeCosStatusFlag.DelayInProgress),
                  ((TypeCosStatusFlag)(s & 0xF8)).HasFlag(TypeCosStatusFlag.NotAttached)
            );
      }
      public static ModulePowerCosDto ParseModulePowerStatus(Guid guid,short c)
      {
            byte codeValue = (byte)(c & 0x07);
            return new ModulePowerCosDto(
                  guid,
                  (DevicePowerFault)codeValue
            );
      }
      public static DoorStatusDto ParseDoorStatus(
            Guid guid,
            short mode,
            short reader,
            short strike,
            short sensor,
            short door,
            short rex0,
            short rex1,
            short ledMode,
            short acsFlag,
            short altrReader,
            short extendAccessControlFlag,
            short extendFeatureType,
            short deviceType,
            string extendFeatureStatus
            )
      {
            return new DoorStatusDto(
                  guid,
                  (DoorMode)mode,
                  (ReaderStatus)reader,
                  StatusTypeCoSDecode(strike),
                  StatusTypeCoSDecode(sensor),
                  StatusTypeCoSDecode(strike),
                  StatusTypeCoSDecode(rex0),
                  StatusTypeCoSDecode(rex1),
                  ledMode,
                  acsFlag,
                  (ReaderStatus)altrReader,
                  extendAccessControlFlag,
                  extendFeatureType,
                  deviceType,
                  extendFeatureStatus
            );
      }
      public static MonitorGroupStatusDto ParseMonitorGroupStatus(
            Guid guid,
            short maskCount,
            short numActive,
            List<short> activeList,
            Dictionary<short, Guid> monitors,
            Dictionary<short, Guid> doors
            )
      {
            var parse = activeList
                  .Chunk(2)
                  .Select(pair => new MonitorGroupComponentStatusDto( (MonitorType)pair[0],(MonitorType)pair[0] == MonitorType.MONITOR_POINT ? monitors[pair[1]] : doors[pair[1]])).ToArray();

            return new MonitorGroupStatusDto(
                  guid,
                  maskCount,
                  numActive,
                  parse
            );
      }
      public static InputCosDto ParseInputStatus(Guid guid,short s)
      {
            return new InputCosDto(
                  guid,
                  StatusTypeCoSDecode(s),
                  ((TypeCosStatusFlag)(s & 0xF8)).HasFlag(TypeCosStatusFlag.Offline),
                  ((TypeCosStatusFlag)(s & 0xF8)).HasFlag(TypeCosStatusFlag.Masked),
                  ((TypeCosStatusFlag)(s & 0xF8)).HasFlag(TypeCosStatusFlag.LocalMask),
                  ((TypeCosStatusFlag)(s & 0xF8)).HasFlag(TypeCosStatusFlag.DelayInProgress),
                  ((TypeCosStatusFlag)(s & 0xF8)).HasFlag(TypeCosStatusFlag.NotAttached)
            );
      }
      public static CommStatus EncodeCommStatus(short d)
      {
            return (CommStatus)d;
      }


      public static ChangeOfState StatusTypeCoSDecode(short b)
      {
            byte codeValue = (byte)(b & 0x07);

            return codeValue switch
            {
                  (byte)ChangeOfState.Inactive => ChangeOfState.Inactive,
                  (byte)ChangeOfState.Active => ChangeOfState.Active,
                  (byte)ChangeOfState.GroudFault => ChangeOfState.GroudFault,
                  (byte)ChangeOfState.Short => ChangeOfState.Short,
                  (byte)ChangeOfState.OpenCircuit => ChangeOfState.OpenCircuit,
                  (byte)ChangeOfState.ForeignVoltage => ChangeOfState.ForeignVoltage,
                  (byte)ChangeOfState.NonSettlingError => ChangeOfState.NonSettlingError,
                  (byte)ChangeOfState.SupervisoryFaultCodes => ChangeOfState.SupervisoryFaultCodes,
                  _ => ChangeOfState.Unknown
            };
      }


}