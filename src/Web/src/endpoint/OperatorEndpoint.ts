const CONTROLLER = `identity/operator`;

export const OperatorEndpoint = {
    GET:(locationId:number) => `/api/${locationId}/${CONTROLLER}`, 
    PAGINATION:(pageNumber:number,pageSize:number,locationId?:number | undefined,search?:string | undefined,startDate?:string | undefined,endDate?:string | undefined) => `/api/${CONTROLLER}/pagination?${locationId == 0 || locationId == undefined ?  "" : `LocationId=${locationId}` }&Page=${pageNumber}&PageSize=${pageSize}${search == undefined || search == "" ? "" : `&Search=${search}`}${startDate == undefined ? "" : `&startDate=${startDate}`}${endDate == undefined ? "" : `&startDate=${endDate}`}`,
    CREATE:`/api/${CONTROLLER}`,
    DELETE:(component:number) => `/api/${CONTROLLER}/${component}`,
    UPDATE:`/api/${CONTROLLER}`,
    GET_ID:(component:number) => `/api/${CONTROLLER}/${component}`,
    PASS: `/api/${CONTROLLER}/password/update`,
    DELETE_RANGE: `/api/${CONTROLLER}/delete/range`
} as const;