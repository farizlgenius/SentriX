"use strict";
// import * as signalR from "@microsoft/signalr";
// import { SignalRHub } from "../constants/signalr-constant";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g = Object.create((typeof Iterator === "function" ? Iterator : Object).prototype);
    return g.next = verb(0), g["throw"] = verb(1), g["return"] = verb(2), typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (g && (g = 0, op[0] && (_ = 0)), _) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
Object.defineProperty(exports, "__esModule", { value: true });
// const PORT = import.meta.env.VITE_RT_PORT || 5031;
// const server = import.meta.env.VITE_SERVER_IP || `${location.protocol}//${location.hostname}:${PORT}`;
// class SignalRService {
//   private connection: signalR.HubConnection | null = null;
//   public getConnection(token:string): signalR.HubConnection {
//     if (!this.connection) {
//       this.connection = new signalR.HubConnectionBuilder()
//         .withUrl(server + SignalRHub.UI,{
//           accessTokenFactory:() => token
//         })
//         .withAutomaticReconnect()
//         .build();
//       // Register lifecycle handlers (for logging & recovery)
//       this.connection.onreconnecting((error) => {
//         console.warn("[SignalR] Reconnecting...", error);
//       });
//       this.connection.onreconnected((connectionId) => {
//         console.info("[SignalR] Reconnected successfully:", connectionId);
//       });
//       this.connection.onclose((error) => {
//         console.error("[SignalR] Connection closed:", error);
//         // Optional: You can auto-restart if desired
//         //this.startConnection();
//       });
//       // Start the connection once
//       this.startConnection();
//     }
//     return this.connection;
//   }
//   private async startConnection() {
//     if (!this.connection) return;
//     try {
//       await this.connection.start();
//       console.info("[SignalR] Connection started successfully.");
//     } catch (error) {
//       console.error("[SignalR] Connection start failed:", error);
//       console.log(server)
//       // Retry after delay if initial connect fails
//       // setTimeout(() => this.startConnection(), 5000);
//     }
//   }
//   public stopConnection() {
//     if (this.connection) {
//       this.connection.stop()
//         .then(() => console.info("[SignalR] Connection stopped."))
//         .catch((error) => console.error("[SignalR] Stop error:", error));
//       this.connection = null;
//     }
//   }
// }
// export default new SignalRService();
var signalR = require("@microsoft/signalr");
var signalr_constant_1 = require("../constants/signalr-constant");
var PORT = import.meta.env.VITE_RT_PORT || 5031;
var server = import.meta.env.VITE_SERVER_IP || "".concat(location.protocol, "//").concat(location.hostname, ":").concat(PORT);
var SignalRService = /** @class */ (function () {
    function SignalRService() {
        this.connection = null;
        this.token = null;
    }
    // ⭐ set/update token anytime (login or refresh)
    SignalRService.prototype.setToken = function (token) {
        this.token = token;
    };
    SignalRService.prototype.startConnection = function () {
        return __awaiter(this, void 0, void 0, function () {
            var error_1;
            var _this = this;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        if (this.connection)
                            return [2 /*return*/];
                        if (!this.token) {
                            console.warn("SignalR: No token yet, skipping start.");
                            return [2 /*return*/];
                        }
                        this.connection = new signalR.HubConnectionBuilder()
                            .withUrl(server + signalr_constant_1.SignalRHub.UI, {
                            accessTokenFactory: function () { return __awaiter(_this, void 0, void 0, function () {
                                return __generator(this, function (_a) {
                                    // ⭐ SignalR will call this BEFORE every HTTP request
                                    console.log("SignalR sending token...");
                                    return [2 /*return*/, this.token];
                                });
                            }); }
                        })
                            .withAutomaticReconnect()
                            .build();
                        this.registerLifecycleEvents();
                        _a.label = 1;
                    case 1:
                        _a.trys.push([1, 3, , 4]);
                        return [4 /*yield*/, this.connection.start()];
                    case 2:
                        _a.sent();
                        console.info("✅ SignalR connected");
                        return [3 /*break*/, 4];
                    case 3:
                        error_1 = _a.sent();
                        console.error("❌ SignalR start failed:", error_1);
                        return [3 /*break*/, 4];
                    case 4: return [2 /*return*/];
                }
            });
        });
    };
    SignalRService.prototype.registerLifecycleEvents = function () {
        var _this = this;
        if (!this.connection)
            return;
        this.connection.onreconnecting(function (err) {
            console.warn("[SignalR] Reconnecting...", err);
        });
        this.connection.onreconnected(function (id) {
            console.info("[SignalR] Reconnected:", id);
        });
        this.connection.onclose(function (err) {
            console.error("[SignalR] Closed:", err);
            _this.connection = null;
        });
    };
    SignalRService.prototype.getConnection = function () {
        return this.connection;
    };
    SignalRService.prototype.joinGroup = function (groupName) {
        return __awaiter(this, void 0, void 0, function () {
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        if (!this.connection)
                            return [2 /*return*/];
                        return [4 /*yield*/, this.connection.invoke("Subscribe", groupName)];
                    case 1:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        });
    };
    SignalRService.prototype.leaveGroup = function (groupName) {
        return __awaiter(this, void 0, void 0, function () {
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        if (!this.connection)
                            return [2 /*return*/];
                        return [4 /*yield*/, this.connection.invoke("Unsubscribe", groupName)];
                    case 1:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        });
    };
    SignalRService.prototype.stopConnection = function () {
        return __awaiter(this, void 0, void 0, function () {
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        if (!this.connection)
                            return [2 /*return*/];
                        return [4 /*yield*/, this.connection.stop()];
                    case 1:
                        _a.sent();
                        this.connection = null;
                        return [2 /*return*/];
                }
            });
        });
    };
    return SignalRService;
}());
exports.default = new SignalRService();
