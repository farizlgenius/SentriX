import { ChangeOfStateEncode } from "../enum/ChangeOfStateEncode";
import { OutputDriverMode as OutputDriveMode } from "../enum/OutputDriveMode";
import { ReaderStatus } from "../enum/ReaderStatus";
import { ActiveIcon, AlarmIcon, DelayIcon, DisconnectIcon, ErrorStatusIcon, FaultIcon, GroundIcon, InactiveIcon, NoneIcon, OfflineIcon, OpenIcon, ReaderIcon, RelayOffIcon, RelayOnIcon, SecureIcon, SensorOffIcon, SensorOnIcon, ShortIcon, SuperIcon, UnknownIcon, VoltageIcon } from "../icons";

export type SvgIconComponent = React.FC<React.SVGProps<SVGSVGElement>>;

export interface StatusDisplay {
  Icon: SvgIconComponent;
  label: string;
  colorClass: "success" | "error" | "warning" | "light" | "info";
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
            colorClass: "success"
          }
          :
          {
            Icon: RelayOffIcon,
            label: "Inactive",
            colorClass: "error"
          }
      case ChangeOfStateEncode.Inactive:
        return mode == OutputDriveMode.NORMAL ?
          {
            Icon: RelayOffIcon,
            label: "Inactive",
            colorClass: "error"
          }
          :
          {
            Icon: RelayOnIcon,
            label: "Active",
            colorClass: "success"
          }
      case ChangeOfStateEncode.GroudFault:
        return {
          Icon: GroundIcon,
          label: "GroudFault",
          colorClass: "error"
        }
      case ChangeOfStateEncode.Short:
        return {
          Icon: ShortIcon,
          label: "Short",
          colorClass: "error"
        }
      case ChangeOfStateEncode.OpenCircuit:
        return {
          Icon: OpenIcon,
          label: "OpenCircuit",
          colorClass: "warning"
        }
      case ChangeOfStateEncode.ForeignVoltage:
        return {
          Icon: VoltageIcon,
          label: "ForeignVoltage",
          colorClass: "warning"
        }
      case ChangeOfStateEncode.NonSettlingError:
        return {
          Icon: ErrorStatusIcon,
          label: "NonSettlingError",
          colorClass: "error"
        }
      case ChangeOfStateEncode.SupervisoryFaultCodes:
        return {
          Icon: SuperIcon,
          label: "SupervisoryFaultCodes",
          colorClass: "warning"
        }
      default:
        return {
          Icon: NoneIcon,
          label: "light",
          colorClass: "light"
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
  //           colorClass: "error"
  //         }
  //         :
  //         {
  //           Icon: RelayOnIcon,
  //           label: "Active",
  //           colorClass: "success"
  //         }
  //     case ChangeOfState.Alarm:
  //       return mode == OutputDriveMode.NORMAL ?
  //         {
  //           Icon: RelayOnIcon,
  //           label: "Active",
  //           colorClass: "success"
  //         }
  //         :
  //         {
  //           Icon: RelayOffIcon,
  //           label: "Inactive",
  //           colorClass: "error"
  //         }
  //     case ChangeOfState.Disconnected:
  //       return {
  //         Icon: DisconnectIcon,
  //         label: "Disconnected",
  //         colorClass: "error"
  //       }
  //     case ChangeOfState.Offline:
  //       return {
  //         Icon: OfflineIcon,
  //         label: "Offline",
  //         colorClass: "error"
  //       }
  //     case ChangeOfState.Fault:
  //       return {
  //         Icon: FaultIcon,
  //         label: "Fault",
  //         colorClass: "warning"
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
  //         colorClass: "warning"
  //       }
  //   }
  // }

  static ReaderStatusParser(s:number) : StatusDisplay {
    switch(s){
      case ReaderStatus.Online:
        return {
          Icon:ReaderIcon,
          label:"Online",
          colorClass:"success"
        }
      case ReaderStatus.Offline:
        return {
          Icon:ReaderIcon,
          label:"Offline",
          colorClass:"error"
        }
      case ReaderStatus.Tamper:
        return {
          Icon:ReaderIcon,
          label:"Tamper",
          colorClass:"warning"
        }
      case ReaderStatus.NA:
        return {
          Icon:ReaderIcon,
          label:"N/A",
          colorClass:"info"
        }
      case ReaderStatus.Unknown:
      default:
        return {
          Icon:UnknownIcon,
          label:"Unknown",
          colorClass:"light"
        }
    }
  }




  static OutputChangeOfStateIconEncode(data: ChangeOfStateEncode): StatusDisplay {
    switch (data) {
      case ChangeOfStateEncode.Active:
        return {
          Icon: RelayOnIcon,
          label: "Active",
          colorClass: "success"
        }
      case ChangeOfStateEncode.Inactive:
        return {
          Icon: RelayOffIcon,
          label: "Inactive",
          colorClass: "error"
        }
      case ChangeOfStateEncode.GroudFault:
        return {
          Icon: GroundIcon,
          label: "GroudFault",
          colorClass: "error"
        }
      case ChangeOfStateEncode.Short:
        return {
          Icon: ShortIcon,
          label: "Short",
          colorClass: "error"
        }
      case ChangeOfStateEncode.OpenCircuit:
        return {
          Icon: OpenIcon,
          label: "OpenCircuit",
          colorClass: "warning"
        }
      case ChangeOfStateEncode.ForeignVoltage:
        return {
          Icon: VoltageIcon,
          label: "ForeignVoltage",
          colorClass: "warning"
        }
      case ChangeOfStateEncode.NonSettlingError:
        return {
          Icon: ErrorStatusIcon,
          label: "NonSettlingError",
          colorClass: "error"
        }
      case ChangeOfStateEncode.SupervisoryFaultCodes:
        return {
          Icon: SuperIcon,
          label: "SupervisoryFaultCodes",
          colorClass: "warning"
        }
      default:
        return {
          Icon: NoneIcon,
          label: "light",
          colorClass: "light"
        }
    }
  }

  static ChangeOfStateIconEncode(data: ChangeOfStateEncode): StatusDisplay {
    switch (data) {
      case ChangeOfStateEncode.Active:
        return {
          Icon: SensorOnIcon,
          label: "Active",
          colorClass: "success"
        }
      case ChangeOfStateEncode.Inactive:
        return {
          Icon: SensorOffIcon,
          label: "Inactive",
          colorClass: "error"
        }
      case ChangeOfStateEncode.GroudFault:
        return {
          Icon: GroundIcon,
          label: "GroudFault",
          colorClass: "error"
        }
      case ChangeOfStateEncode.Short:
        return {
          Icon: ShortIcon,
          label: "Short",
          colorClass: "error"
        }
      case ChangeOfStateEncode.OpenCircuit:
        return {
          Icon: OpenIcon,
          label: "OpenCircuit",
          colorClass: "warning"
        }
      case ChangeOfStateEncode.ForeignVoltage:
        return {
          Icon: VoltageIcon,
          label: "ForeignVoltage",
          colorClass: "warning"
        }
      case ChangeOfStateEncode.NonSettlingError:
        return {
          Icon: ErrorStatusIcon,
          label: "NonSettlingError",
          colorClass: "error"
        }
      case ChangeOfStateEncode.SupervisoryFaultCodes:
        return {
          Icon: SuperIcon,
          label: "SupervisoryFaultCodes",
          colorClass: "warning"
        }
      default:
        return {
          Icon: NoneIcon,
          label: "light",
          colorClass: "light"
        }
    }
  }


}
