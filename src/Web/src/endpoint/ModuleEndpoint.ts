const CONTROLLER = `device/module`;

export const ModuleEndpoint = {
    GET:(deviceId:number) => `/api/${CONTROLLER}/${deviceId}`,
    PAGINATION:(pageNumber:number,pageSize:number,locationId?:number | undefined,search?:string | undefined,startDate?:string | undefined,endDate?:string | undefined) => `/api${locationId == 0 || locationId == undefined ?  "" : `/${locationId}` }/${CONTROLLER}/pagination?PageNumber=${pageNumber}&PageSize=${pageSize}${search == undefined || search == "" ? "" : `&search=${search}`}${startDate == undefined ? "" : `&startDate=${startDate}`}${endDate == undefined ? "" : `&startDate=${endDate}`}`,
    CREATE:`/api/${CONTROLLER}`,
    GET_ID:(id:number) => `/api/${CONTROLLER}/${id}`,
    GET_BY_DEVICE_ID:(deviceOd:number) => `/api/${CONTROLLER}/option/${deviceOd}`,
    STATUS:(moduleId:number) => `/api/${CONTROLLER}/status/${moduleId}`,
    BAUDRATE: `/api/${CONTROLLER}/baudrate`,
    PROTOCOL : `/api/${CONTROLLER}/protocol`

} as const;