const CONTROLLER = `operator`;

export const OperatorEndpoint = {
    GET_PASSWORD:`/api/${CONTROLLER}/password/rule`,
    UPDATE_PASSWORD:`/api/${CONTROLLER}/password/rule`,
    GET:(locationId:number) => `/api/${locationId}/${CONTROLLER}`, 
    PAGINATION:(pageNumber:number,pageSize:number,locationGuid?:string | undefined,search?:string | undefined,startDate?:string | undefined,endDate?:string | undefined) => `/api/${CONTROLLER}/pagination?Page=${pageNumber}&PageSize=${pageSize}${locationGuid !== undefined ? `&LocationGuid=${locationGuid}` : ""}${search == undefined || search == "" ? "" : `&Search=${search}`}${startDate == undefined ? "" : `&startDate=${startDate}`}${endDate == undefined ? "" : `&startDate=${endDate}`}`,
    CREATE:`/api/${CONTROLLER}`,
    DELETE:(guid:string) => `/api/${CONTROLLER}/${guid}`,
    UPDATE:`/api/${CONTROLLER}`,
    PASS: `/api/${CONTROLLER}/password/update`,
    DELETE_RANGE: `/api/${CONTROLLER}/delete/range`
} as const;