namespace Adapter.Aero.Enums;

public enum NakReason
{
      InvalidPacketHeader,
      InvalidCommandType_1,
      InvalidCommandType_2,
      InvalidCommandType_3,
      CommandContentError,
      CannotExecute_RequirePassword,
      StandbyMode,
      FailedLogon,
      CommandNotExcept
}
