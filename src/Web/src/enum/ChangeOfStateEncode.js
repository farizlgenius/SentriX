"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.ChangeOfStateEncode = void 0;
var ChangeOfStateEncode;
(function (ChangeOfStateEncode) {
    ChangeOfStateEncode[ChangeOfStateEncode["Inactive"] = 0] = "Inactive";
    ChangeOfStateEncode[ChangeOfStateEncode["Active"] = 1] = "Active";
    ChangeOfStateEncode[ChangeOfStateEncode["GroudFault"] = 2] = "GroudFault";
    ChangeOfStateEncode[ChangeOfStateEncode["Short"] = 3] = "Short";
    ChangeOfStateEncode[ChangeOfStateEncode["OpenCircuit"] = 4] = "OpenCircuit";
    ChangeOfStateEncode[ChangeOfStateEncode["ForeignVoltage"] = 5] = "ForeignVoltage";
    ChangeOfStateEncode[ChangeOfStateEncode["NonSettlingError"] = 6] = "NonSettlingError";
    ChangeOfStateEncode[ChangeOfStateEncode["SupervisoryFaultCodes"] = 7] = "SupervisoryFaultCodes";
    ChangeOfStateEncode[ChangeOfStateEncode["Unknown"] = 8] = "Unknown";
})(ChangeOfStateEncode || (exports.ChangeOfStateEncode = ChangeOfStateEncode = {}));
