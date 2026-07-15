import { DeviceType } from "../enum/DeviceType";

const CONTROLLER = `device`;

export const DeviceEndpoint = {
    GET:(locationId:number,type:string) => `/api/${CONTROLLER}/option/${type}/${locationId}`,
    GET_OPTION_BY_TYPE:(locationId:number,type:string) => `/api/${CONTROLLER}/option/${type}/${locationId}`,
    PAGINATION:(pageNumber:number,pageSize:number,locationId?:number | undefined,search?:string | undefined,startDate?:string | undefined,endDate?:string | undefined) => `/api/${CONTROLLER}/pagination?PageNumber=${pageNumber}&PageSize=${pageSize}${search == undefined || search == "" ? "" : `&search=${search}`}${startDate == undefined ? "" : `&startDate=${startDate}`}${endDate == undefined ? "" : `&startDate=${endDate}`}${locationId == undefined ? "" : `&locationId=${locationId}`}`,
    TYPE: `/api/${CONTROLLER}/type`,
    DELETE:(guid:string) => `/api/${CONTROLLER}/${guid}`,
    DELETE_RANGE: `/api/${CONTROLLER}/range`,
    STATUS:(guid:string) => `/api/${CONTROLLER}/status/${guid}`,
    RESET:(guid:string) => `/api/${CONTROLLER}/reset/${guid}`,
    UPLOAD:(guid:string) => `/api/${CONTROLLER}/upload/${guid}`,
    CREATE : `/api/${CONTROLLER}`,
    UPDATE : `/api/${CONTROLLER}`,
    VERIFY_MEM:(mac:string) => `/api/${CONTROLLER}/verify/mem/${mac}`,
    VERIFY_COM:(mac:string) => `/api/${CONTROLLER}/verify/com/${mac}`,
    GET_EVENT_STATUS:(guid:string) => `/api/${CONTROLLER}/event/${guid}`,
    ID_REPORT: `/api/${CONTROLLER}/report`,
    SET_TRAN : `/api/${CONTROLLER}/event`,
    TRAN_RANGE: `/api/${CONTROLLER}/tran/range`,
    GET_READER:(moduleId:number) => `/api/${CONTROLLER}/module/reader/options/${moduleId}`,
    GET_INPUT:(moduleId:number) => `/api/${CONTROLLER}/module/input/options/${moduleId}`,
    GET_RELAY:(moduleId:number) => `/api/${CONTROLLER}/module/relay/options/${moduleId}`,
    CHECK_AMICO_CONNECT: `/api/${CONTROLLER}/${DeviceType.AMICO}/connect`
} as const

