import { ChangeOfState } from "../enum/ChangeOfState";
import { ChangeOfStateEncode } from "../enum/ChangeOfStateEncode";
import { OutputDriverMode as OutputDriveMode } from "../enum/OutputDriveMode";
import { RelayMode } from "../enum/RelayMode";
import { ActiveIcon, AlarmIcon, DelayIcon, DisconnectIcon, ErrorStatusIcon, FaultIcon, GroundIcon, InactiveIcon, NoneIcon, OfflineIcon, OpenIcon, RelayOffIcon, RelayOnIcon, SecureIcon, SensorOffIcon, SensorOnIcon, ShortIcon, SuperIcon, UnknownIcon, VoltageIcon } from "../icons";

export type SvgIconComponent = React.FC<React.SVGProps<SVGSVGElement>>;

export interface StatusDisplay {
  Icon: SvgIconComponent;
  label: string;
  colorClass: string;
}

export class StatusHelper {
  // Prevent instantiation
  private constructor() { }

  static OutputChangeOfStateIcon(data: ChangeOfStateEncode,mode:OutputDriveMode): StatusDisplay {
    switch (data) {
      case ChangeOfStateEncode.Active:
        return mode == OutputDriveMode.NORMAL ?
          {
            Icon: RelayOnIcon,
            label: "Active",
            colorClass: "green"
          }
          :
          {
            Icon: RelayOffIcon,
            label: "Inactive",
            colorClass: "red"
          }
      case ChangeOfStateEncode.Inactive:
        return mode == OutputDriveMode.NORMAL ?
          {
            Icon: RelayOffIcon,
            label: "Inactive",
            colorClass: "red"
          }
          :
          {
            Icon: RelayOnIcon,
            label: "Active",
            colorClass: "green"
          }
      case ChangeOfStateEncode.GroudFault:
        return {
          Icon: GroundIcon,
          label: "GroudFault",
          colorClass: "red"
        }
      case ChangeOfStateEncode.Short:
        return {
          Icon: ShortIcon,
          label: "Short",
          colorClass: "red"
        }
      case ChangeOfStateEncode.OpenCircuit:
        return {
          Icon: OpenIcon,
          label: "OpenCircuit",
          colorClass: "yellow"
        }
      case ChangeOfStateEncode.ForeignVoltage:
        return {
          Icon: VoltageIcon,
          label: "ForeignVoltage",
          colorClass: "yellow"
        }
      case ChangeOfStateEncode.NonSettlingError:
        return {
          Icon: ErrorStatusIcon,
          label: "NonSettlingError",
          colorClass: "red"
        }
      case ChangeOfStateEncode.SupervisoryFaultCodes:
        return {
          Icon: SuperIcon,
          label: "SupervisoryFaultCodes",
          colorClass: "yellow"
        }
      default:
        return {
          Icon: NoneIcon,
          label: "",
          colorClass: ""
        }
    }
  }

  // static OutputChangeOfStateIcon(data: ChangeOfState, mode: OutputDriveMode): StatusDisplay {
  //   switch (data) {
  //     case ChangeOfState.Secure:
  //       return mode == OutputDriveMode.NORMAL ?
  //         {
  //           Icon: RelayOffIcon,
  //           label: "Inactive",
  //           colorClass: "red"
  //         }
  //         :
  //         {
  //           Icon: RelayOnIcon,
  //           label: "Active",
  //           colorClass: "green"
  //         }
  //     case ChangeOfState.Alarm:
  //       return mode == OutputDriveMode.NORMAL ?
  //         {
  //           Icon: RelayOnIcon,
  //           label: "Active",
  //           colorClass: "green"
  //         }
  //         :
  //         {
  //           Icon: RelayOffIcon,
  //           label: "Inactive",
  //           colorClass: "red"
  //         }
  //     case ChangeOfState.Disconnected:
  //       return {
  //         Icon: DisconnectIcon,
  //         label: "Disconnected",
  //         colorClass: "red"
  //       }
  //     case ChangeOfState.Offline:
  //       return {
  //         Icon: OfflineIcon,
  //         label: "Offline",
  //         colorClass: "red"
  //       }
  //     case ChangeOfState.Fault:
  //       return {
  //         Icon: FaultIcon,
  //         label: "Fault",
  //         colorClass: "yellow"
  //       }
  //     case ChangeOfState.ExitDelayInProgress:
  //     case ChangeOfState.EntryDelayInProgress:
  //       return {
  //         Icon: DelayIcon,
  //         label: "Delay",
  //         colorClass: "text-brand-500"
  //       }
  //     default:
  //       return {
  //         Icon: UnknownIcon,
  //         label: "Unknown",
  //         colorClass: "yellow"
  //       }
  //   }
  // }


  static ChangeOfStateIcon(data: ChangeOfState): StatusDisplay {
    switch (data) {
      case ChangeOfState.Disconnected:
        return {
          Icon: DisconnectIcon,
          label: "Disconnected",
          colorClass: "red"
        }
      case ChangeOfState.Offline:
        return {
          Icon: OfflineIcon,
          label: "Offline",
          colorClass: "red"
        }
      case ChangeOfState.Secure:
        return {
          Icon: SecureIcon,
          label: "Secure",
          colorClass: "green"
        }
      case ChangeOfState.Alarm:
        return {
          Icon: AlarmIcon,
          label: "Alarm",
          colorClass: "yellow"
        }
      case ChangeOfState.Fault:
        return {
          Icon: FaultIcon,
          label: "Fault",
          colorClass: "yellow"
        }
      case ChangeOfState.ExitDelayInProgress:
      case ChangeOfState.EntryDelayInProgress:
        return {
          Icon: DelayIcon,
          label: "Delay",
          colorClass: "text-brand-500"
        }
      case ChangeOfState.Unknown:
      default:
        return {
          Icon: NoneIcon,
          label: "",
          colorClass: ""
        }
    }
  }

  static ChangeOfStateIconEncode(data: ChangeOfStateEncode): StatusDisplay {
    switch (data) {
      case ChangeOfStateEncode.Active:
        return {
          Icon: SensorOnIcon,
          label: "Active",
          colorClass: "green"
        }
      case ChangeOfStateEncode.Inactive:
        return {
          Icon: SensorOffIcon,
          label: "Inactive",
          colorClass: "red"
        }
      case ChangeOfStateEncode.GroudFault:
        return {
          Icon: GroundIcon,
          label: "GroudFault",
          colorClass: "red"
        }
      case ChangeOfStateEncode.Short:
        return {
          Icon: ShortIcon,
          label: "Short",
          colorClass: "red"
        }
      case ChangeOfStateEncode.OpenCircuit:
        return {
          Icon: OpenIcon,
          label: "OpenCircuit",
          colorClass: "yellow"
        }
      case ChangeOfStateEncode.ForeignVoltage:
        return {
          Icon: VoltageIcon,
          label: "ForeignVoltage",
          colorClass: "yellow"
        }
      case ChangeOfStateEncode.NonSettlingError:
        return {
          Icon: ErrorStatusIcon,
          label: "NonSettlingError",
          colorClass: "red"
        }
      case ChangeOfStateEncode.SupervisoryFaultCodes:
        return {
          Icon: SuperIcon,
          label: "SupervisoryFaultCodes",
          colorClass: "yellow"
        }
      default:
        return {
          Icon: NoneIcon,
          label: "",
          colorClass: ""
        }
    }
  }


}

