"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Logger = /** @class */ (function () {
    function Logger() {
    }
    Logger.timestamp = function () {
        return new Date().toISOString();
    };
    Logger.info = function (e) {
        if (e instanceof Error) {
            console.error("[".concat(this.timestamp(), "] [ERROR]: ").concat(e.message));
        }
        else {
            console.info("[".concat(this.timestamp(), "] [INFO]: ").concat(String(e)));
        }
    };
    Logger.warn = function (e) {
        if (e instanceof Error) {
            console.error("[".concat(this.timestamp(), "] [ERROR]: ").concat(e.message));
        }
        else {
            console.warn("[".concat(this.timestamp(), "] [WARN]: ").concat(String(e)));
        }
    };
    Logger.error = function (e) {
        if (e instanceof Error) {
            console.error("[".concat(this.timestamp(), "] [ERROR]: ").concat(e.message));
        }
        else {
            console.error("[".concat(this.timestamp(), "] [ERROR]: ").concat(String(e)));
        }
    };
    return Logger;
}());
exports.default = Logger;
