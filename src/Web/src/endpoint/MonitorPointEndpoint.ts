const CONTROLLER = `input`;

export const MonitorPointEndpoint = {
    GET:(locationGuid:number)=> `/api/${locationGuid}/${CONTROLLER}`,
    GET_BY_DEVICE:(guid:string) => `/api/${CONTROLLER}/device/${guid}`,
    PAGINATION:(pageNumber:number,pageSize:number,locationGuid?:string | undefined,search?:string | undefined,startDate?:string | undefined,endDate?:string | undefined) => `/api/${CONTROLLER}/pagination?PageNumber=${pageNumber}&PageSize=${pageSize}${search == undefined || search == "" ? "" : `&search=${search}`}${startDate == undefined ? "" : `&startDate=${startDate}`}${endDate == undefined ? "" : `&startDate=${endDate}`}${locationGuid == "" || locationGuid == undefined ?  "" : `&locationGuid=${locationGuid}` }`,
    MP_BY_MAC:(mac:string) =>  `/api/${CONTROLLER}/${mac}`,
    CREATE : `/api/${CONTROLLER}`,
    UPDATE:  `/api/${CONTROLLER}`,
    GET_MP_STATUS:(guid:string) => `/api/${CONTROLLER}/status/${guid}`,
    DELETE :(guid:string) => `/api/${CONTROLLER}/${guid}`,
    DELETE_RANGE: `/api/${CONTROLLER}/delete/range`,
    IP_LIST :(moduleId:number) => `/api/${CONTROLLER}/input/${moduleId}`,
    MASK : `/api/${CONTROLLER}/mask`,
    UNMASK : `/api/${CONTROLLER}/unmask`,
    IP_MODE : `/api/${CONTROLLER}/mode`,
    MP_MODE : `/api/${CONTROLLER}/mode`,
    LOG_FUNCTION : `/api/${CONTROLLER}/lf`
} as const;