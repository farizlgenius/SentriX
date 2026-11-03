const API_VERSION = import.meta.env.VITE_API_VERSION;
const CONTROLLER = `MonitorGroup`;

export const MonitorGroupEndpoint = {
    GET :(location:number) => `/api/${location}/${CONTROLLER}`,
    PAGINATION:(pageNumber:number,pageSize:number,locationGuid?:string | undefined,search?:string | undefined,startDate?:string | undefined,endDate?:string | undefined) => `/api/${CONTROLLER}/pagination?PageNumber=${pageNumber}&PageSize=${pageSize}${search == undefined || search == "" ? "" : `&search=${search}`}${startDate == undefined ? "" : `&startDate=${startDate}`}${endDate == undefined ? "" : `&startDate=${endDate}`}${locationGuid == "" || locationGuid == undefined ?  "" : `&locationGuid=${locationGuid}` }`,
    GET_TYPE: `/api/${CONTROLLER}/type`,
    CREATE: `/api/${CONTROLLER}`,
    UPDATE : `/api/${CONTROLLER}`,
    DELETE_RANGE: `/api/${CONTROLLER}/delete/range`,
    COMMAND: `/api/${CONTROLLER}/command`,
    DELETE:(guid: string) => `/api/${CONTROLLER}/${guid}`,
    STATUS:(guid:string) => `/api/${CONTROLLER}/status/${guid}`
} as const;