const CONTROLLER = `position`;

export const PositionEndpoint = {
    GET: `/api/${CONTROLLER}`,
    GET_BY_LOCATION:(location:number) => `/api/${location}/${CONTROLLER}`,
    GET_BY_DEPARTMENT:(departmentId:number) => `/api/${CONTROLLER}/department/${departmentId}`,
    GET_OPTION_BY_DEPARTMENT:(departmentId:number) => `/api/${CONTROLLER}/option/department/${departmentId}`,
    CREATE: `/api/${CONTROLLER}`,
    PAGINATION_BY_DEPART : (pageNumber: number, pageSize: number,departmentId:number,locationId?: number | undefined, search?: string | undefined, startDate?: string | undefined, endDate?: string | undefined) => `/api/${CONTROLLER}/pagination/${departmentId}?Page=${pageNumber}&PageSize=${pageSize}${search == undefined || search == "" ? "" : `&search=${search}`}${startDate == undefined ? "" : `&startDate=${startDate}`}${endDate == undefined ? "" : `&startDate=${endDate}`}${locationId == 0 || locationId == undefined ? "" : `&locationId=${locationId}`}`, 
    PAGINATION: (pageNumber: number, pageSize: number, locationId?: number | undefined, search?: string | undefined, startDate?: string | undefined, endDate?: string | undefined) => `/api/${CONTROLLER}/pagination?${locationId == 0 || locationId == undefined ? "" : `locationId=${locationId}`}&Page=${pageNumber}&PageSize=${pageSize}${search == undefined || search == "" ? "" : `&search=${search}`}${startDate == undefined ? "" : `&startDate=${startDate}`}${endDate == undefined ? "" : `&startDate=${endDate}`}`,
    UPDATE: `/api/${CONTROLLER}`,
    DELETE: (component: number) => `/api/${CONTROLLER}/${component}`,
    GET_RANGE: `/api/${CONTROLLER}/range`,
    DELETE_RANGE: `/api/${CONTROLLER}/delete/range`
} as const;
