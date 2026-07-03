
const CONTROLLER = "event";

export const EventEndpoint = {
    GET_PAGINATION:(pageNumber:number,pageSize:number,locationId:number,search?:string,startDate?:string,endDate?:string) => `/api/${CONTROLLER}/pagination?PageNumber=${pageNumber}&PageSize=${pageSize}${search == undefined || search == "" ? "" : `&search=${search}`}${startDate == undefined ? "" : `&startDate=${startDate}`}${endDate == undefined ? "" : `&endDate=${endDate}`}${locationId == undefined ? "" : `&locationId=${locationId}`}`,
    SOURCE: `/api/${CONTROLLER}/source`,
    DEVICE:(source:number) => `/api/${CONTROLLER}/device/${source}`,
    GET_COMMAND_PAGINATION:(pageNumber:number,pageSize:number,locationId:number,search?:string,startDate?:string,endDate?:string) => `/api/${CONTROLLER}/command/pagination?PageNumber=${pageNumber}&PageSize=${pageSize}${search == undefined || search == "" ? "" : `&search=${search}`}${startDate == undefined ? "" : `&startDate=${startDate}`}${endDate == undefined ? "" : `&endDate=${endDate}`}${locationId == undefined ? "" : `&locationId=${locationId}`}`,
} as const;