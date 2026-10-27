const CONTROLLER = `area`;

export const AreaEndpoint = {
    GET:(locationGuid:number) => `/api/${locationGuid}/${CONTROLLER}`, 
    PAGINATION:(pageNumber:number,pageSize:number,locationGuid?:string | undefined,search?:string | undefined,startDate?:string | undefined,endDate?:string | undefined) => `/api/${CONTROLLER}/pagination?PageNumber=${pageNumber}&PageSize=${pageSize}${search == undefined || search == "" ? "" : `&search=${search}`}${startDate == undefined ? "" : `&startDate=${startDate}`}${endDate == undefined ? "" : `&startDate=${endDate}`}${locationGuid == "" || locationGuid == undefined ?  "" : `&locationGuid=${locationGuid}` }`,
    CREATE:`/api/${CONTROLLER}`,
    DELETE:(guid:string) => `/api/${CONTROLLER}/${guid}`,
    DELETE_RANGE: `/api/${CONTROLLER}`,
    UPDATE:`/api/${CONTROLLER}`,
    GET_ACCESS_CONTROL:`/api/${CONTROLLER}/access`,
    GET_OCC_CONTROL:`/api/${CONTROLLER}/occcontrol`,
    GET_ARE_FLAG:`/api/${CONTROLLER}/areaflag`,
    GET_MULTI_OCC:`/api/${CONTROLLER}/multiocc`,
} as const;