const CONTROLLER = `alert`;

export const AlertEndpoint = {
   PAGINATION:(pageNumber:number,pageSize:number,locationGuid?:string | undefined,search?:string | undefined,startDate?:string | undefined,endDate?:string | undefined) => `/api/${CONTROLLER}/pagination?PageNumber=${pageNumber}&PageSize=${pageSize}${search == undefined || search == "" ? "" : `&search=${search}`}${startDate == undefined ? "" : `&startDate=${startDate}`}${endDate == undefined ? "" : `&startDate=${endDate}`}${locationGuid == "" || locationGuid == undefined ?  "" : `&locationGuid=${locationGuid}` }`,
   PAGINATION_LEVEL:(pageNumber:number,pageSize:number,locationGuid?:string | undefined,search?:string | undefined,startDate?:string | undefined,endDate?:string | undefined) => `/api/${CONTROLLER}/level/pagination?PageNumber=${pageNumber}&PageSize=${pageSize}${search == undefined || search == "" ? "" : `&search=${search}`}${startDate == undefined ? "" : `&startDate=${startDate}`}${endDate == undefined ? "" : `&startDate=${endDate}`}${locationGuid == "" || locationGuid == undefined ?  "" : `&locationGuid=${locationGuid}` }`,
   CREATE_LEVEL:`/api/${CONTROLLER}/level`, 
   CREATE:`/api/${CONTROLLER}`,
    UPDATE: `/api/${CONTROLLER}`,
    UPDATE_LEVEL: `/api/${CONTROLLER}/level`,
    DELETE:(guid:string) => `/api/${CONTROLLER}/${guid}`,
    DELETE_LEVEL:(guid:string) => `/api/${CONTROLLER}/level/${guid}`,
    DLETE_RANGE :`/api/${CONTROLLER}`,
    ACTION_TYPE : `/api/${CONTROLLER}/type`
} as const;