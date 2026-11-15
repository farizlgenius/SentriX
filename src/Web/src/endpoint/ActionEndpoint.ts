import { ActionType } from "../enum/ActionType";

const CONTROLLER = `action`;

export const ActionEndpoint = {
    GET:(location:number) => `/api/${location}/${CONTROLLER}`,
    GET_OPTION_BY_DEVICE_AND_TYPE: (guid:string,type:ActionType) => `/api/${CONTROLLER}/option/type/${type}/device/${guid}`,
    GET_OPTION_BY_DEVICE: (guid:string) => `/api/${CONTROLLER}/option/device/${guid}`,
   PAGINATION:(pageNumber:number,pageSize:number,locationGuid?:string | undefined,search?:string | undefined,startDate?:string | undefined,endDate?:string | undefined) => `/api/${CONTROLLER}/pagination?PageNumber=${pageNumber}&PageSize=${pageSize}${search == undefined || search == "" ? "" : `&search=${search}`}${startDate == undefined ? "" : `&startDate=${startDate}`}${endDate == undefined ? "" : `&startDate=${endDate}`}${locationGuid == "" || locationGuid == undefined ?  "" : `&locationGuid=${locationGuid}` }`,
    CREATE:`/api/${CONTROLLER}`,
    UPDATE: `/api/${CONTROLLER}`,
    DELETE:(guid:string) => `/api/${CONTROLLER}/${guid}`,
    DLETE_RANGE :`/api/${CONTROLLER}`,
    ACTION_TYPE : `/api/${CONTROLLER}/type`
} as const;