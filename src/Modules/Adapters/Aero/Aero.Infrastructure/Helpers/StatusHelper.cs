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
      public static InputStatusDto ParseInputStatus(Guid guid,short s)
      {
            return new InputStatusDto(
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
                  (byte)MonitorStatus.Inactive => ChangeOfState.Inactive,
                  (byte)MonitorStatus.Active => ChangeOfState.Active,
                  (byte)MonitorStatus.GroundFault => ChangeOfState.GroudFault,
                  (byte)MonitorStatus.Short => ChangeOfState.Short,
                  (byte)MonitorStatus.OpenCircuit => ChangeOfState.OpenCircuit,
                  (byte)MonitorStatus.ForeignVoltage => ChangeOfState.ForeignVoltage,
                  (byte)MonitorStatus.NonSettlingError => ChangeOfState.NonSettlingError,
                  (byte)MonitorStatus.SupervisoryFault => ChangeOfState.SupervisoryFaultCodes,
                  _ => ChangeOfState.Unknown
            };
      }

      public static ChangeOfStateCode TranCodeTypeCos(short b)
      {
            return b switch
            {
                  (short)ChangeOfStateCode.Alarm => ChangeOfStateCode.Alarm,
                  (short)ChangeOfStateCode.Disconnected => ChangeOfStateCode.Alarm,
                  (short)ChangeOfStateCode.EntryDelayInProgress => ChangeOfStateCode.EntryDelayInProgress,
                  (short)ChangeOfStateCode.ExitDelayInProgress => ChangeOfStateCode.ExitDelayInProgress,
                  (short)ChangeOfStateCode.Fault => ChangeOfStateCode.Fault,
                  (short)ChangeOfStateCode.Offline => ChangeOfStateCode.Offline,
                  (short)ChangeOfStateCode.Secure => ChangeOfStateCode.Secure,
                  _ => ChangeOfStateCode.Unknown
            };
      } 
}