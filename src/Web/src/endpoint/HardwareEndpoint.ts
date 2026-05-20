const CONTROLLER = `device`;

export const DeviceEndpoint = {
    GET:(locationId:number) => `/api/${CONTROLLER}/option/${locationId}`,
    PAGINATION:(pageNumber:number,pageSize:number,locationId?:number | undefined,search?:string | undefined,startDate?:string | undefined,endDate?:string | undefined) => `/api/${CONTROLLER}/pagination?PageNumber=${pageNumber}&PageSize=${pageSize}${search == undefined || search == "" ? "" : `&search=${search}`}${startDate == undefined ? "" : `&startDate=${startDate}`}${endDate == undefined ? "" : `&startDate=${endDate}`}${locationId == undefined ? "" : `&locationId=${locationId}`}`,
    TYPE: `/api/${CONTROLLER}/type`,
    DELETE:(id:number) => `/api/${CONTROLLER}/${id}`,
    DELETE_RANGE: `/api/${CONTROLLER}/range`,
    STATUS:(id:number) => `/api/${CONTROLLER}/status/${id}`,
    RESET:(id:number) => `/api/${CONTROLLER}/reset/${id}`,
    UPLOAD:(id:number) => `/api/${CONTROLLER}/upload/${id}`,
    CREATE : `/api/${CONTROLLER}`,
    UPDATE : `/api/${CONTROLLER}`,
    VERIFY_MEM:(mac:string) => `/api/${CONTROLLER}/verify/mem/${mac}`,
    VERIFY_COM:(mac:string) => `/api/${CONTROLLER}/verify/com/${mac}`,
    TRAN:(mac:string) => `/api/${CONTROLLER}/tran/${mac}`,
    ID_REPORT: `/api/${CONTROLLER}/report`,
    SET_TRAN : (mac:string,param:number) => `/api/${CONTROLLER}/tran/${mac}/${param}`,
    TRAN_RANGE: `/api/${CONTROLLER}/tran/range`
} as const

