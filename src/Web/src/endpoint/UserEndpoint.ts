const CONTROLLER = 'user'

// CREDENTIAL
export const UserEndpoint = {
    GET:(locationId:number)=> `/api/${locationId}/${CONTROLLER}`,
     PAGINATION:(pageNumber:number,pageSize:number,locationId?:number | undefined,search?:string | undefined,startDate?:string | undefined,endDate?:string | undefined) => `/api/${CONTROLLER}/pagination?PageNumber=${pageNumber}&PageSize=${pageSize}${search == undefined || search == "" ? "" : `&search=${search}`}${startDate == undefined ? "" : `&startDate=${startDate}`}${endDate == undefined ? "" : `&startDate=${endDate}`}${locationId == 0 || locationId == undefined ?  "" : `&locationId=${locationId}` }`,
    DELETE:(id:number)=> `/api/${CONTROLLER}/${id}`,
    UPDATE:`/api/${CONTROLLER}`, 
    CREATE:`/api/${CONTROLLER}`,
    SCAN:`/api/${CONTROLLER}/scan`,
    DELETE_RANGE: `/api/${CONTROLLER}/delete/range`,
    UPLOAD:(userid:string) =>`/api/${CONTROLLER}/image/upload/${userid}`,
    IMAGE:(userId: string) => `/api/${CONTROLLER}/image/${userId}`,
} as const
