import { ChangeOfState } from "../enum/ChangeOfState";
import { ChangeOfStateEncode } from "../enum/ChangeOfStateEncode";
import { ActiveIcon, AlarmIcon, DelayIcon, DisconnectIcon, ErrorStatusIcon, FaultIcon, GroundIcon, InactiveIcon, NoneIcon, OfflineIcon, OpenIcon, SecureIcon, ShortIcon, SuperIcon, UnknownIcon, VoltageIcon } from "../icons";

export type SvgIconComponent = React.FC<React.SVGProps<SVGSVGElement>>;

export interface StatusDisplay {
  Icon: SvgIconComponent;
  label: string;
  colorClass: string;
}

export class StatusHelper {
  // Prevent instantiation
  private constructor() { }


  static ChangeOfStateIcon(data: ChangeOfState): StatusDisplay {
    switch (data) {
      case ChangeOfState.Disconnected:
        return {
          Icon: DisconnectIcon,
          label: "Disconnected",
          colorClass: "text-red-500"
        }
      case ChangeOfState.Offline:
        return {
          Icon: OfflineIcon,
          label: "Offline",
          colorClass: "text-red-500"
        }
      case ChangeOfState.Secure:
        return {
          Icon: SecureIcon,
          label: "Secure",
          colorClass: "text-green-500"
        }
      case ChangeOfState.Alarm:
        return {
          Icon: AlarmIcon,
          label: "Alarm",
          colorClass: "text-yellow-500"
        }
      case ChangeOfState.Fault:
        return {
          Icon: FaultIcon,
          label: "Fault",
          colorClass: "text-yellow-500"
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
          Icon: UnknownIcon,
          label: "Unknown",
          colorClass: "text-yellow-500"
        }
    }
  }

  static ChangeOfStateIconEncode(data: ChangeOfStateEncode): StatusDisplay {
    switch (data) {
      case ChangeOfStateEncode.Active:
        return {
          Icon: ActiveIcon,
          label: "Active",
          colorClass: "text-green-500"
        }
      case ChangeOfStateEncode.Inactive:
        return {
          Icon: InactiveIcon,
          label: "Inactive",
          colorClass: "text-brand-500"
        }
      case ChangeOfStateEncode.GroudFault:
        return {
          Icon: GroundIcon,
          label: "GroudFault",
          colorClass: "text-red-500"
        }
      case ChangeOfStateEncode.Short:
        return {
          Icon: ShortIcon,
          label: "Short",
          colorClass: "text-red-500"
        }
      case ChangeOfStateEncode.OpenCircuit:
        return {
          Icon: OpenIcon,
          label: "OpenCircuit",
          colorClass: "text-yellow-500"
        }
      case ChangeOfStateEncode.ForeignVoltage:
        return {
          Icon: VoltageIcon,
          label: "ForeignVoltage",
          colorClass: "text-yellow-500"
        }
      case ChangeOfStateEncode.NonSettlingError:
        return {
          Icon: ErrorStatusIcon,
          label: "NonSettlingError",
          colorClass: "text-red-500"
        }
      case ChangeOfStateEncode.SupervisoryFaultCodes:
        return {
          Icon: SuperIcon,
          label: "SupervisoryFaultCodes",
          colorClass: "text-yellow-500"
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

