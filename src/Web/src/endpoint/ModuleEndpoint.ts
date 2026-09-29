import { Vendor } from "../enum/Vendor";

const CONTROLLER = `devicemodule`;

export const ModuleEndpoint = {
  GET_BY_VENDOR: (guid: string, vendor: Vendor) =>
    `/api/${CONTROLLER}/location/${guid}/vendor/${vendor}`,
  GET_BY_GUID: (guid: string) => `/api/${CONTROLLER}/device/${guid}`,
  PAGINATION: (
    pageNumber: number,
    pageSize: number,
    locationId?: number | undefined,
    search?: string | undefined,
    startDate?: string | undefined,
    endDate?: string | undefined,
  ) =>
    `/api${locationId == 0 || locationId == undefined ? "" : `/${locationId}`}/${CONTROLLER}/pagination?PageNumber=${pageNumber}&PageSize=${pageSize}${search == undefined || search == "" ? "" : `&search=${search}`}${startDate == undefined ? "" : `&startDate=${startDate}`}${endDate == undefined ? "" : `&startDate=${endDate}`}`,
  CREATE: `/api/${CONTROLLER}`,
  GET_ID: (id: number) => `/api/${CONTROLLER}/${id}`,
  GET_BY_DEVICE_ID: (deviceId: number) =>
    `/api/${CONTROLLER}/option/${deviceId}`,
  STATUS: (moduleId: number) => `/api/${CONTROLLER}/status/${moduleId}`,
  BAUDRATE: `/api/${CONTROLLER}/baudrate`,
  PROTOCOL: `/api/${CONTROLLER}/protocol`,
  GET_READER_SLOT:(guid:string) => `/api/${CONTROLLER}/reader/${guid}`,
  GET_INPUT_SLOT:(guid:string) => `/api/${CONTROLLER}/input/${guid}` ,
  GET_OUtPUT_SLOT:(guid:string) => `/api/${CONTROLLER}/output/${guid}` 
} as const;
