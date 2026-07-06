const CONTROLLER = 'setting/cfmt'

export const CardFormatEndpoint = {
    GET : `/api/${CONTROLLER}`,
    PAGINATION:(pageNumber:number,pageSize:number,locationId?:number | undefined,search?:string | undefined,startDate?:string | undefined,endDate?:string | undefined) => `/api/${CONTROLLER}/pagination?PageNumber=${pageNumber}&PageSize=${pageSize}${search == undefined || search == "" ? "" : `&search=${search}`}${startDate == undefined ? "" : `&startDate=${startDate}`}${endDate == undefined ? "" : `&startDate=${endDate}`}${locationId == 0 || locationId == undefined ?  "" : `&locationId=${locationId}` }`,
    CREATE : `/api/${CONTROLLER}`,
    UPDATE : `/api/${CONTROLLER}`,
    DELETE :(component:number) => `/api/${CONTROLLER}/${component}`,
    DELETE_RANGE : `/api/${CONTROLLER}/delete/range`
}
