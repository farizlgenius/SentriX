const CONTROLLER = `position`;

export const PositionEndpoint = {
  GET: `/api/${CONTROLLER}`,
  GET_BY_LOCATION: (location: number) => `/api/${location}/${CONTROLLER}`,
  GET_BY_DEPARTMENT: (guid: string) => `/api/${CONTROLLER}/department/${guid}`,
  GET_OPTION_BY_DEPARTMENT: (departmentId: number) =>
    `/api/${CONTROLLER}/option/department/${departmentId}`,
  CREATE: `/api/${CONTROLLER}`,
  PAGINATION_BY_DEPART: (
    pageNumber: number,
    pageSize: number,
    departmentGuid: string,
    search?: string | undefined,
    startDate?: string | undefined,
    endDate?: string | undefined,
  ) =>
    `/api/${CONTROLLER}/pagination/${departmentGuid}?Page=${pageNumber}&PageSize=${pageSize}${search == undefined || search == "" ? "" : `&search=${search}`}${startDate == undefined ? "" : `&startDate=${startDate}`}${endDate == undefined ? "" : `&startDate=${endDate}`}`,
  PAGINATION: (
    pageNumber: number,
    pageSize: number,
    locationId?: number | undefined,
    search?: string | undefined,
    startDate?: string | undefined,
    endDate?: string | undefined,
  ) =>
    `/api/${CONTROLLER}/pagination?${locationId == 0 || locationId == undefined ? "" : `locationId=${locationId}`}&Page=${pageNumber}&PageSize=${pageSize}${search == undefined || search == "" ? "" : `&search=${search}`}${startDate == undefined ? "" : `&startDate=${startDate}`}${endDate == undefined ? "" : `&startDate=${endDate}`}`,
  UPDATE: `/api/${CONTROLLER}`,
  DELETE: (guid: string) => `/api/${CONTROLLER}/${guid}`,
  GET_RANGE: `/api/${CONTROLLER}/range`,
  DELETE_RANGE: `/api/${CONTROLLER}/list`,
} as const;
