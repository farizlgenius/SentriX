const CONTROLLER = `action`;

export const ActionEndpoint = {
    GET:(location:number) => `/api/${location}/${CONTROLLER}`,
   PAGINATION:(pageNumber:number,pageSize:number,locationGuid?:string | undefined,search?:string | undefined,startDate?:string | undefined,endDate?:string | undefined) => `/api/${CONTROLLER}/pagination?PageNumber=${pageNumber}&PageSize=${pageSize}${search == undefined || search == "" ? "" : `&search=${search}`}${startDate == undefined ? "" : `&startDate=${startDate}`}${endDate == undefined ? "" : `&startDate=${endDate}`}${locationGuid == "" || locationGuid == undefined ?  "" : `&locationGuid=${locationGuid}` }`,
    CREATE:`/api/${CONTROLLER}`,
    UPDATE: `/api/${CONTROLLER}`,
    DELETE:(guid:string) => `/api/${CONTROLLER}/${guid}`,
    DLETE_RANGE :`/api/${CONTROLLER}`,
    ACTION_TYPE : `/api/${CONTROLLER}/type`
} as const;