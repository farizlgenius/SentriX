const CONTROLLER = `output`;

export const OutputEndpoint = {
    GET:(location:number) => `/api/${location}/${CONTROLLER}`,
    GET_DEVICE:(guid:string) => `/api/${CONTROLLER}/device/${guid}`,
    DELETE :(guid:string) => `/api/${CONTROLLER}/${guid}`,
    PAGINATION:(pageNumber:number,pageSize:number,locationGuid?:string | undefined,search?:string | undefined,startDate?:string | undefined,endDate?:string | undefined) => `/api/${CONTROLLER}/pagination?PageNumber=${pageNumber}&PageSize=${pageSize}${search == undefined || search == "" ? "" : `&search=${search}`}${startDate == undefined ? "" : `&startDate=${startDate}`}${endDate == undefined ? "" : `&startDate=${endDate}`}${locationGuid == undefined ?  "" : `&locationGuid=${locationGuid}` }`,
    TRIGGER : `/api/${CONTROLLER}/command`,
    CREATE : `/api/${CONTROLLER}`,
    UPDATE : `/api/${CONTROLLER}`,
    STATUS :(guid:string)=> `/api/${CONTROLLER}/status/${guid}`,
    OUTPUT : (module:number) => `/api/${CONTROLLER}/relay/${module}`,
    DELETE_RANGE: `/api/${CONTROLLER}/delete/range`,
    RELAY_DRIVE_MODE:`/api/${CONTROLLER}/relay/drive/mode`,
    RELAY_OFFLINE_MODE: `/api/${CONTROLLER}/relay/offline/mode`
} as const;

