import axios, { AxiosError, AxiosResponse } from "axios";
import { DaysInWeekDto } from "../model/Interval/DaysInWeekDto";
import { Options } from "../model/Options";
import { HttpCode } from "../enum/Httpcode";
import { APIToast } from "../model/ToastMessage";

type ToastType = "success" | "error" | "warning" | "pending";
type ToastFn = (
  toastType: ToastType,
  toastMessage: string,
  options?: { duration?: number },
) => string;
type UpdateToastFn = (
  id: string,
  updates: {
    type?: ToastType;
    message?: string;
    duration?: number;
  },
) => void;

class Helper {
  static handlePopupByResCode(
    res: AxiosResponse<any, any> | null | undefined,
    showPopup: (isSuccess: boolean, popUpMessage: string[]) => void,
  ): boolean {
    if (res) {
      switch (res.data.code) {
        case HttpCode.OK:
          showPopup(true, []);
          return true;
        case HttpCode.CREATED:
          showPopup(true, []);
          return true;
        case HttpCode.BAD_REQUEST:
          showPopup(false, res.data.message);
          return false;
        case HttpCode.INTERNAL_ERROR:
          showPopup(false, [res.data.detail, res.data.message]);
          console.log(res.data);
          return false;
        default:
          return false;
      }
    } else {
      showPopup(false, [APIToast.API_ERROR]);
      console.log("####");
      return false;
    }
  }

  static handleToastByResCode(
    res: AxiosResponse<any> | AxiosError<any> | null | undefined,
    message: string,
    toggleToast: ToastFn,
    updateToast?: UpdateToastFn,
    toastId?: string,
  ): boolean {
    console.log("handleToastByResCode input:", res);

    const notify = (
      type: ToastType,
      toastMessage: string,
      duration = type === "success" ? 2600 : 4000,
    ) => {
      if (toastId && updateToast) {
        updateToast(toastId, {
          type,
          message: toastMessage,
          duration,
        });
        return;
      }

      toggleToast(type, toastMessage, { duration });
    };

    if (!res) {
      notify("error", APIToast.API_ERROR);
      return false;
    }

    // Handle Axios Error (HTTP 4xx/5xx responses)
    if (axios.isAxiosError(res)) {
      const status = res.response?.status;
      const data = res.response?.data;

      console.log(res.response);

      // Helper to safely extract error message from common response shapes
      const errorMessage =
        data?.errors?.exception ||
        data?.message ||
        data?.detail ||
        data?.title ||
        res.message ||
        "An unexpected error occurred";

      switch (status) {
        case HttpCode.UNAUTHORIZED:
          notify("error", errorMessage || "Unauthorized");
          return false;
        case HttpCode.BAD_REQUEST:
          notify("error", errorMessage || "Bad Request");
          return false;
        case HttpCode.NOT_FOUND:
          notify("error", errorMessage || "Not Found");
          return false;
        case HttpCode.NOT_ACCEPT:
          notify("error", errorMessage || "Not Acceptable");
          return false;
        case HttpCode.INTERNAL_ERROR:
          notify("error", errorMessage || "Internal Server Error");
          return false;
        default:
          notify("error", errorMessage);
          return false;
      }
    }

    const errorMessage =
      res.data?.errors?.exception ||
      res.data?.message ||
      res.data?.detail ||
      res.data?.title ||
      "An unexpected error occurred";

    // Handle Successful Axios Responses (HTTP 2xx)
    const status = res.status;
    switch (status) {
      case HttpCode.OK:
      case HttpCode.CREATED:
        notify("success", message);
        return true;
      case HttpCode.UNAUTHORIZED:
        notify("error", errorMessage || "Unauthorized");
        return false;
      case HttpCode.BAD_REQUEST:
        notify("error", errorMessage || "Bad Request");
        return false;
      case HttpCode.NOT_FOUND:
        notify("error", errorMessage || "Not Found");
        return false;
      case HttpCode.NOT_ACCEPT:
        notify("error", errorMessage || "Not Acceptable");
        return false;
      case HttpCode.INTERNAL_ERROR:
        notify("error", errorMessage || "Internal Server Error");
        return false;
      default:
        notify("error", errorMessage);
        return false;
    }
  }

  static isValidTimeRange(start: string, end: string): boolean {
    // Assume format "HH:mm"
    const [sh, sm] = start.split(":").map(Number);
    const [eh, em] = end.split(":").map(Number);

    const startMinutes = sh * 60 + sm;
    const endMinutes = eh * 60 + em;

    return startMinutes < endMinutes;
  }

  // static isValidTimeRange(start: Date, end: Date): boolean {
  //   return start < end;
  // }

  static isDayEmpty(data: DaysInWeekDto): boolean {
    if (
      data.sunday ||
      data.monday ||
      data.tuesday ||
      data.wednesday ||
      data.thursday ||
      data.friday ||
      data.saturday
    ) {
      return true;
    }
    return false;
  }

  static updateOptionByValue(
    array: Options[],
    value: string | number,
    isTaken: boolean,
  ): Options[] {
    const index = array.findIndex((item) => item.value === value);
    if (index === -1) return array; // not found → return original

    const updated = [...array]; // shallow copy
    updated[index] = { ...updated[index], isTaken }; // update only isTaken
    return updated;
  }

  static toCapitalCase(str: string): string {
    if (!str) return "";
    return str.charAt(0).toUpperCase() + str.slice(1).toLowerCase();
  }

  static fileToBase64(file: File): Promise<string> {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();

      reader.readAsDataURL(file);

      reader.onload = () => {
        if (typeof reader.result === "string") {
          resolve(reader.result);
        } else {
          reject("Failed to convert file to Base64");
        }
      };

      reader.onerror = (error) => reject(error);
    });
  }
}

export default Helper;
