const CONTROLLER = `Trigger`;

export const TriggerEndpoint = {
    GET:(locationId:number) => `/api/${locationId}/${CONTROLLER}`,
    GET_TRIGGER_SOURCE: `/api/${CONTROLLER}/source`,
    GET_TRIGGER_TYPE:(source:string) => `/api/${CONTROLLER}/type/${source}`,
    PAGINATION:(pageNumber:number,pageSize:number,locationGuid?:string | undefined,search?:string | undefined,startDate?:string | undefined,endDate?:string | undefined) => `/api/${CONTROLLER}/pagination?PageNumber=${pageNumber}&PageSize=${pageSize}${search == undefined || search == "" ? "" : `&search=${search}`}${startDate == undefined ? "" : `&startDate=${startDate}`}${endDate == undefined ? "" : `&startDate=${endDate}`}${locationGuid == "" || locationGuid == undefined ?  "" : `&locationGuid=${locationGuid}` }`,
    COMMAND : `/api/${CONTROLLER}/command`,
    SOURCE : `/api/${CONTROLLER}/source`,
    TRAN:(source:number) => `/api/${CONTROLLER}/tran/${source}`,
    CODE:(type:number) => `/api/${CONTROLLER}/code/${type}`,
    DEVICE:(location:number,source:number) => `/api/${location}/${CONTROLLER}/device/${source}`,
    CREATE:`/api/${CONTROLLER}`,
    DELETE:(id:string) => `/api/${CONTROLLER}/${id}`,
    DELETE_RANGE: `/api/${CONTROLLER}`,
    UPDATE:`/api/${CONTROLLER}`,
} as const;