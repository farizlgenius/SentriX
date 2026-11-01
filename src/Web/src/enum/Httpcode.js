"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.HttpCode = void 0;
// HttpCode
var HttpCode;
(function (HttpCode) {
    HttpCode[HttpCode["CREATED"] = 201] = "CREATED";
    HttpCode[HttpCode["OK"] = 200] = "OK";
    HttpCode[HttpCode["BAD_REQUEST"] = 400] = "BAD_REQUEST";
    HttpCode[HttpCode["NOT_FOUND"] = 404] = "NOT_FOUND";
    HttpCode[HttpCode["INTERNAL_ERROR"] = 500] = "INTERNAL_ERROR";
    HttpCode[HttpCode["NOT_ACCEPT"] = 406] = "NOT_ACCEPT";
    HttpCode[HttpCode["UNAUTHORIZED"] = 401] = "UNAUTHORIZED";
})(HttpCode || (exports.HttpCode = HttpCode = {}));
