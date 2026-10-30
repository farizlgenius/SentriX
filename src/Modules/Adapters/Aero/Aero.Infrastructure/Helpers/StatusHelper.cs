using Aero.Application.Enums;
using Aero.Domain.Entities;
using HID.Aero.ScpdNet.Wrapper;
using SharedKernel.Enums;
using SharedKernel.Model.Status;

namespace Aero.Infrastructure.Helpers;

public static class StatusHelper
{
      public static OutputStatusDto ParseOutputStatus(Guid guid,short c,short s)
      {
            return new OutputStatusDto(
                  guid,
                  TranCodeTypeCos(c),
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