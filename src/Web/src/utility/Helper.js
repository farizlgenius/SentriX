"use strict";
var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
var __spreadArray = (this && this.__spreadArray) || function (to, from, pack) {
    if (pack || arguments.length === 2) for (var i = 0, l = from.length, ar; i < l; i++) {
        if (ar || !(i in from)) {
            if (!ar) ar = Array.prototype.slice.call(from, 0, i);
            ar[i] = from[i];
        }
    }
    return to.concat(ar || Array.prototype.slice.call(from));
};
Object.defineProperty(exports, "__esModule", { value: true });
var axios_1 = require("axios");
var Httpcode_1 = require("../enum/Httpcode");
var ToastMessage_1 = require("../model/ToastMessage");
var Helper = /** @class */ (function () {
    function Helper() {
    }
    Helper.handlePopupByResCode = function (res, showPopup) {
        if (res) {
            switch (res.data.code) {
                case Httpcode_1.HttpCode.OK:
                    showPopup(true, []);
                    return true;
                case Httpcode_1.HttpCode.CREATED:
                    showPopup(true, []);
                    return true;
                case Httpcode_1.HttpCode.BAD_REQUEST:
                    showPopup(false, res.data.message);
                    return false;
                case Httpcode_1.HttpCode.INTERNAL_ERROR:
                    showPopup(false, [res.data.detail, res.data.message]);
                    console.log(res.data);
                    return false;
                default:
                    return false;
            }
        }
        else {
            showPopup(false, [ToastMessage_1.APIToast.API_ERROR]);
            console.log("####");
            return false;
        }
    };
    Helper.handleToastByResCode = function (res, message, toggleToast, updateToast, toastId) {
        var _a, _b, _c, _d, _e, _f, _g, _h;
        console.log("handleToastByResCode input:", res);
        var notify = function (type, toastMessage, duration) {
            if (duration === void 0) { duration = type === "success" ? 2600 : 4000; }
            if (toastId && updateToast) {
                updateToast(toastId, {
                    type: type,
                    message: toastMessage,
                    duration: duration,
                });
                return;
            }
            toggleToast(type, toastMessage, { duration: duration });
        };
        if (!res) {
            notify("error", ToastMessage_1.APIToast.API_ERROR);
            return false;
        }
        // Handle Axios Error (HTTP 4xx/5xx responses)
        if (axios_1.default.isAxiosError(res)) {
            var status_1 = (_a = res.response) === null || _a === void 0 ? void 0 : _a.status;
            var data = (_b = res.response) === null || _b === void 0 ? void 0 : _b.data;
            console.log(res.response);
            // Helper to safely extract error message from common response shapes
            var errorMessage_1 = ((_c = data === null || data === void 0 ? void 0 : data.errors) === null || _c === void 0 ? void 0 : _c.exception) ||
                (data === null || data === void 0 ? void 0 : data.message) ||
                (data === null || data === void 0 ? void 0 : data.detail) ||
                (data === null || data === void 0 ? void 0 : data.title) ||
                res.message ||
                "An unexpected error occurred";
            switch (status_1) {
                case Httpcode_1.HttpCode.UNAUTHORIZED:
                    notify("error", errorMessage_1 || "Unauthorized");
                    return false;
                case Httpcode_1.HttpCode.BAD_REQUEST:
                    notify("error", errorMessage_1 || "Bad Request");
                    return false;
                case Httpcode_1.HttpCode.NOT_FOUND:
                    notify("error", errorMessage_1 || "Not Found");
                    return false;
                case Httpcode_1.HttpCode.NOT_ACCEPT:
                    notify("error", errorMessage_1 || "Not Acceptable");
                    return false;
                case Httpcode_1.HttpCode.INTERNAL_ERROR:
                    notify("error", errorMessage_1 || "Internal Server Error");
                    return false;
                default:
                    notify("error", errorMessage_1);
                    return false;
            }
        }
        var errorMessage = ((_e = (_d = res.data) === null || _d === void 0 ? void 0 : _d.errors) === null || _e === void 0 ? void 0 : _e.exception) ||
            ((_f = res.data) === null || _f === void 0 ? void 0 : _f.message) ||
            ((_g = res.data) === null || _g === void 0 ? void 0 : _g.detail) ||
            ((_h = res.data) === null || _h === void 0 ? void 0 : _h.title) ||
            "An unexpected error occurred";
        // Handle Successful Axios Responses (HTTP 2xx)
        var status = res.status;
        switch (status) {
            case Httpcode_1.HttpCode.OK:
            case Httpcode_1.HttpCode.CREATED:
                notify("success", message);
                return true;
            case Httpcode_1.HttpCode.UNAUTHORIZED:
                notify("error", errorMessage || "Unauthorized");
                return false;
            case Httpcode_1.HttpCode.BAD_REQUEST:
                notify("error", errorMessage || "Bad Request");
                return false;
            case Httpcode_1.HttpCode.NOT_FOUND:
                notify("error", errorMessage || "Not Found");
                return false;
            case Httpcode_1.HttpCode.NOT_ACCEPT:
                notify("error", errorMessage || "Not Acceptable");
                return false;
            case Httpcode_1.HttpCode.INTERNAL_ERROR:
                notify("error", errorMessage || "Internal Server Error");
                return false;
            default:
                notify("error", errorMessage);
                return false;
        }
    };
    Helper.isValidTimeRange = function (start, end) {
        // Assume format "HH:mm"
        var _a = start.split(":").map(Number), sh = _a[0], sm = _a[1];
        var _b = end.split(":").map(Number), eh = _b[0], em = _b[1];
        var startMinutes = sh * 60 + sm;
        var endMinutes = eh * 60 + em;
        return startMinutes < endMinutes;
    };
    // static isValidTimeRange(start: Date, end: Date): boolean {
    //   return start < end;
    // }
    Helper.isDayEmpty = function (data) {
        if (data.sunday ||
            data.monday ||
            data.tuesday ||
            data.wednesday ||
            data.thursday ||
            data.friday ||
            data.saturday) {
            return true;
        }
        return false;
    };
    Helper.updateOptionByValue = function (array, value, isTaken) {
        var index = array.findIndex(function (item) { return item.value === value; });
        if (index === -1)
            return array; // not found → return original
        var updated = __spreadArray([], array, true); // shallow copy
        updated[index] = __assign(__assign({}, updated[index]), { isTaken: isTaken }); // update only isTaken
        return updated;
    };
    Helper.toCapitalCase = function (str) {
        if (!str)
            return "";
        return str.charAt(0).toUpperCase() + str.slice(1).toLowerCase();
    };
    Helper.fileToBase64 = function (file) {
        return new Promise(function (resolve, reject) {
            var reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = function () {
                if (typeof reader.result === "string") {
                    resolve(reader.result);
                }
                else {
                    reject("Failed to convert file to Base64");
                }
            };
            reader.onerror = function (error) { return reject(error); };
        });
    };
    return Helper;
}());
exports.default = Helper;
