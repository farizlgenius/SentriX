"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.ChangeOfState = void 0;
var ChangeOfState;
(function (ChangeOfState) {
    ChangeOfState[ChangeOfState["Disconnected"] = 1] = "Disconnected";
    ChangeOfState[ChangeOfState["Offline"] = 2] = "Offline";
    ChangeOfState[ChangeOfState["Secure"] = 3] = "Secure";
    ChangeOfState[ChangeOfState["Alarm"] = 4] = "Alarm";
    ChangeOfState[ChangeOfState["Fault"] = 5] = "Fault";
    ChangeOfState[ChangeOfState["ExitDelayInProgress"] = 6] = "ExitDelayInProgress";
    ChangeOfState[ChangeOfState["EntryDelayInProgress"] = 7] = "EntryDelayInProgress";
    ChangeOfState[ChangeOfState["Unknown"] = 8] = "Unknown";
})(ChangeOfState || (exports.ChangeOfState = ChangeOfState = {}));
