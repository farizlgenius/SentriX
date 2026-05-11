const CONTROLLER = `identity/role`;

export const RoleEndpoint = {
    GET:`/api/${CONTROLLER}`,
    GET_BY_LOCATION :(location:number) => `/api/${CONTROLLER}/location/${location}`,
    CREATE:`/api/${CONTROLLER}`,
    PAGINATION:(pageNumber:number,pageSize:number,locationId?:number | undefined,search?:string | undefined,startDate?:string | undefined,endDate?:string | undefined) => `/api/${CONTROLLER}/pagination?${locationId == 0 || locationId == undefined ?  "" : `LocationId=${locationId}` }&Page=${pageNumber}&PageSize=${pageSize}${search == undefined || search == "" ? "" : `&search=${search}`}${startDate == undefined ? "" : `&startDate=${startDate}`}${endDate == undefined ? "" : `&startDate=${endDate}`}`,
    DELETE :(component:number) => `/api/${CONTROLLER}/${component}`,
    UPDATE :`/api/${CONTROLLER}`,
    GET_FEATURE :`/api/${CONTROLLER}/feature`,
    DELETE_RANGE:`/api/${CONTROLLER}/delete/range`
} as const;