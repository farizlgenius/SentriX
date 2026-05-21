const CONTROLLER = 'time/timezone'


export const TimeZoneEndPoint = {
    GET : `/api/${CONTROLLER}`,
    LOCATION:(location:number) => `/api/${location}/${CONTROLLER}`,
    PAGINATION:(pageNumber:number,pageSize:number,locationId?:number | undefined,search?:string | undefined,startDate?:string | undefined,endDate?:string | undefined) => `/api/${CONTROLLER}/pagination?${locationId == 0 || locationId == undefined ?  "" : `LocationId=${locationId}` }&PageNumber=${pageNumber}&PageSize=${pageSize}${search == undefined || search == "" ? "" : `&search=${search}`}${startDate == undefined ? "" : `&startDate=${startDate}`}${endDate == undefined ? "" : `&startDate=${endDate}`}`,
    GET_ID :(component:number) => `/api/${CONTROLLER}/${component}`,
    DELETE :(component:number) => `/api/${CONTROLLER}/${component}`,
    DELETE_RANGE: `/api/${CONTROLLER}/delete/range`,
    UPDATE: `/api/${CONTROLLER}`,
    CREATE : `/api/${CONTROLLER}`,
    GET_MODE : (Type:string) => `/api/${CONTROLLER}/mode?Type=${Type}`,
    COMMAND: `/api/${CONTROLLER}/command`
} as const;