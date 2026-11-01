"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.StatusHelper = void 0;
var ChangeOfState_1 = require("../enum/ChangeOfState");
var ChangeOfStateEncode_1 = require("../enum/ChangeOfStateEncode");
var OutputDriveMode_1 = require("../enum/OutputDriveMode");
var icons_1 = require("../icons");
var StatusHelper = /** @class */ (function () {
    // Prevent instantiation
    function StatusHelper() {
    }
    StatusHelper.OutputChangeOfStateIcon = function (data, mode) {
        switch (data) {
            case ChangeOfStateEncode_1.ChangeOfStateEncode.Active:
                return mode == OutputDriveMode_1.OutputDriverMode.NORMAL ?
                    {
                        Icon: icons_1.RelayOnIcon,
                        label: "Active",
                        colorClass: "green"
                    }
                    :
                        {
                            Icon: icons_1.RelayOffIcon,
                            label: "Inactive",
                            colorClass: "red"
                        };
            case ChangeOfStateEncode_1.ChangeOfStateEncode.Inactive:
                return mode == OutputDriveMode_1.OutputDriverMode.NORMAL ?
                    {
                        Icon: icons_1.RelayOffIcon,
                        label: "Inactive",
                        colorClass: "red"
                    }
                    :
                        {
                            Icon: icons_1.RelayOnIcon,
                            label: "Active",
                            colorClass: "green"
                        };
            case ChangeOfStateEncode_1.ChangeOfStateEncode.GroudFault:
                return {
                    Icon: icons_1.GroundIcon,
                    label: "GroudFault",
                    colorClass: "red"
                };
            case ChangeOfStateEncode_1.ChangeOfStateEncode.Short:
                return {
                    Icon: icons_1.ShortIcon,
                    label: "Short",
                    colorClass: "red"
                };
            case ChangeOfStateEncode_1.ChangeOfStateEncode.OpenCircuit:
                return {
                    Icon: icons_1.OpenIcon,
                    label: "OpenCircuit",
                    colorClass: "yellow"
                };
            case ChangeOfStateEncode_1.ChangeOfStateEncode.ForeignVoltage:
                return {
                    Icon: icons_1.VoltageIcon,
                    label: "ForeignVoltage",
                    colorClass: "yellow"
                };
            case ChangeOfStateEncode_1.ChangeOfStateEncode.NonSettlingError:
                return {
                    Icon: icons_1.ErrorStatusIcon,
                    label: "NonSettlingError",
                    colorClass: "red"
                };
            case ChangeOfStateEncode_1.ChangeOfStateEncode.SupervisoryFaultCodes:
                return {
                    Icon: icons_1.SuperIcon,
                    label: "SupervisoryFaultCodes",
                    colorClass: "yellow"
                };
            default:
                return {
                    Icon: icons_1.NoneIcon,
                    label: "",
                    colorClass: ""
                };
        }
    };
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
    StatusHelper.ChangeOfStateIcon = function (data) {
        switch (data) {
            case ChangeOfState_1.ChangeOfState.Disconnected:
                return {
                    Icon: icons_1.DisconnectIcon,
                    label: "Disconnected",
                    colorClass: "red"
                };
            case ChangeOfState_1.ChangeOfState.Offline:
                return {
                    Icon: icons_1.OfflineIcon,
                    label: "Offline",
                    colorClass: "red"
                };
            case ChangeOfState_1.ChangeOfState.Secure:
                return {
                    Icon: icons_1.SecureIcon,
                    label: "Secure",
                    colorClass: "green"
                };
            case ChangeOfState_1.ChangeOfState.Alarm:
                return {
                    Icon: icons_1.AlarmIcon,
                    label: "Alarm",
                    colorClass: "yellow"
                };
            case ChangeOfState_1.ChangeOfState.Fault:
                return {
                    Icon: icons_1.FaultIcon,
                    label: "Fault",
                    colorClass: "yellow"
                };
            case ChangeOfState_1.ChangeOfState.ExitDelayInProgress:
            case ChangeOfState_1.ChangeOfState.EntryDelayInProgress:
                return {
                    Icon: icons_1.DelayIcon,
                    label: "Delay",
                    colorClass: "text-brand-500"
                };
            case ChangeOfState_1.ChangeOfState.Unknown:
            default:
                return {
                    Icon: icons_1.NoneIcon,
                    label: "",
                    colorClass: ""
                };
        }
    };
    StatusHelper.ChangeOfStateIconEncode = function (data) {
        switch (data) {
            case ChangeOfStateEncode_1.ChangeOfStateEncode.Active:
                return {
                    Icon: icons_1.SensorOnIcon,
                    label: "Active",
                    colorClass: "green"
                };
            case ChangeOfStateEncode_1.ChangeOfStateEncode.Inactive:
                return {
                    Icon: icons_1.SensorOffIcon,
                    label: "Inactive",
                    colorClass: "red"
                };
            case ChangeOfStateEncode_1.ChangeOfStateEncode.GroudFault:
                return {
                    Icon: icons_1.GroundIcon,
                    label: "GroudFault",
                    colorClass: "red"
                };
            case ChangeOfStateEncode_1.ChangeOfStateEncode.Short:
                return {
                    Icon: icons_1.ShortIcon,
                    label: "Short",
                    colorClass: "red"
                };
            case ChangeOfStateEncode_1.ChangeOfStateEncode.OpenCircuit:
                return {
                    Icon: icons_1.OpenIcon,
                    label: "OpenCircuit",
                    colorClass: "yellow"
                };
            case ChangeOfStateEncode_1.ChangeOfStateEncode.ForeignVoltage:
                return {
                    Icon: icons_1.VoltageIcon,
                    label: "ForeignVoltage",
                    colorClass: "yellow"
                };
            case ChangeOfStateEncode_1.ChangeOfStateEncode.NonSettlingError:
                return {
                    Icon: icons_1.ErrorStatusIcon,
                    label: "NonSettlingError",
                    colorClass: "red"
                };
            case ChangeOfStateEncode_1.ChangeOfStateEncode.SupervisoryFaultCodes:
                return {
                    Icon: icons_1.SuperIcon,
                    label: "SupervisoryFaultCodes",
                    colorClass: "yellow"
                };
            default:
                return {
                    Icon: icons_1.NoneIcon,
                    label: "",
                    colorClass: ""
                };
        }
    };
    return StatusHelper;
}());
exports.StatusHelper = StatusHelper;
