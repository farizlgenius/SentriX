"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.MonitorPointEndpoint = void 0;
var CONTROLLER = "input";
exports.MonitorPointEndpoint = {
    GET: function (locationGuid) { return "/api/".concat(locationGuid, "/").concat(CONTROLLER); },
    GET_BY_DEVICE: function (guid) { return "/api/".concat(CONTROLLER, "/device/").concat(guid); },
    PAGINATION: function (pageNumber, pageSize, locationGuid, search, startDate, endDate) { return "/api/".concat(CONTROLLER, "/pagination?PageNumber=").concat(pageNumber, "&PageSize=").concat(pageSize).concat(search == undefined || search == "" ? "" : "&search=".concat(search)).concat(startDate == undefined ? "" : "&startDate=".concat(startDate)).concat(endDate == undefined ? "" : "&startDate=".concat(endDate)).concat(locationGuid == "" || locationGuid == undefined ? "" : "&locationGuid=".concat(locationGuid)); },
    MP_BY_MAC: function (mac) { return "/api/".concat(CONTROLLER, "/").concat(mac); },
    CREATE: "/api/".concat(CONTROLLER),
    UPDATE: "/api/".concat(CONTROLLER),
    GET_MP_STATUS: function (guid) { return "/api/".concat(CONTROLLER, "/status/").concat(guid); },
    DELETE: function (guid) { return "/api/".concat(CONTROLLER, "/").concat(guid); },
    DELETE_RANGE: "/api/".concat(CONTROLLER, "/delete/range"),
    IP_LIST: function (moduleId) { return "/api/".concat(CONTROLLER, "/input/").concat(moduleId); },
    MASK: function (guid) { return "/api/".concat(CONTROLLER, "/mask/").concat(guid); },
    UNMASK: function (guid) { return "/api/".concat(CONTROLLER, "/unmask/").concat(guid); },
    IP_MODE: "/api/".concat(CONTROLLER, "/mode"),
    MP_MODE: "/api/".concat(CONTROLLER, "/mode"),
    LOG_FUNCTION: "/api/".concat(CONTROLLER, "/lf")
};
