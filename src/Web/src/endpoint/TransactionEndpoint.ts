
const CONTROLLER = "event";

export const EventEndpoint = {
    CAPTURE:(time:string) => `/api/${CONTROLLER}/capture/${time}`,
    GET_PAGINATION:(pageNumber:number,pageSize:number,locationId:number,search?:string,startDate?:string,endDate?:string) => `/api/${CONTROLLER}/pagination?PageNumber=${pageNumber}&PageSize=${pageSize}${search == undefined || search == "" ? "" : `&search=${search}`}${startDate == undefined ? "" : `&startDate=${startDate}`}${endDate == undefined ? "" : `&endDate=${endDate}`}${locationId == undefined ? "" : `&locationId=${locationId}`}`,
    SOURCE: `/api/${CONTROLLER}/source`,
    DEVICE:(source:number) => `/api/${CONTROLLER}/device/${source}`,
    GET_ADAPTER_PAGINATION:(pageNumber:number,pageSize:number,locationGuid:string,search?:string,startDate?:string,endDate?:string) => `/api/${CONTROLLER}/adapter/pagination?PageNumber=${pageNumber}&PageSize=${pageSize}${search == undefined || search == "" ? "" : `&search=${search}`}${startDate == undefined ? "" : `&startDate=${startDate}`}${endDate == undefined ? "" : `&endDate=${endDate}`}${locationGuid == undefined ? "" : `&locationGuid=${locationGuid}`}`,
     GET_EXCEPTION_PAGINATION:(pageNumber:number,pageSize:number,locationGuid:string,search?:string,startDate?:string,endDate?:string) => `/api/${CONTROLLER}/exception/pagination?PageNumber=${pageNumber}&PageSize=${pageSize}${search == undefined || search == "" ? "" : `&search=${search}`}${startDate == undefined ? "" : `&startDate=${startDate}`}${endDate == undefined ? "" : `&endDate=${endDate}`}${locationGuid == undefined ? "" : `&locationGuid=${locationGuid}`}`,
} as const;