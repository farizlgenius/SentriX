"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.SignalRHub = exports.SignalRTopic = void 0;
var SignalRTopic;
(function (SignalRTopic) {
    SignalRTopic["IDREPORT"] = "DEVICE.IDREPORT";
    SignalRTopic["MODULE_STATUS"] = "DEVICE.MODULE.STATUS";
    SignalRTopic["OUTPUT_STATUS"] = "OUTPUT.STATUS";
    SignalRTopic["INPUT_STATUS"] = "INPUT.STATUS";
    SignalRTopic["DEVICE_STATUS"] = "DEVICE.STATUS";
    SignalRTopic["DEVICE_EVENT_STATUS"] = "DEVICE.EVENT.STATUS";
    SignalRTopic["EVENT"] = "EVENT";
    SignalRTopic["EXCEPTION_EVENT"] = "EXCEPTION.EVENT";
    SignalRTopic["ADAPTER_EVENT"] = "ADAPTER.EVENT";
    SignalRTopic["CONFIG"] = "DEVICE.CONFIG";
    SignalRTopic["DOOR_STATUS"] = "DOOR.STATUS";
})(SignalRTopic || (exports.SignalRTopic = SignalRTopic = {}));
var SignalRHub;
(function (SignalRHub) {
    SignalRHub["UI"] = "/notiHubs";
})(SignalRHub || (exports.SignalRHub = SignalRHub = {}));
