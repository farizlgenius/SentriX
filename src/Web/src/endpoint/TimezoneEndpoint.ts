const CONTROLLER = "time/timezone";

export const TimezoneEndPoint = {
  GET: `/api/${CONTROLLER}`,
  GET_OPTION_BY_LOCATION: (locationId: number) =>
    `/api/${CONTROLLER}/option/${locationId}`,
  LOCATION: (location: number) => `/api/${location}/${CONTROLLER}`,
  PAGINATION: (
    pageNumber: number,
    pageSize: number,
    locationGuid?: string | undefined,
    search?: string | undefined,
    startDate?: string | undefined,
    endDate?: string | undefined,
  ) =>
    `/api/${CONTROLLER}/pagination?${locationGuid == "" || locationGuid == undefined ? "" : `LocationGuid=${locationGuid}`}&PageNumber=${pageNumber}&PageSize=${pageSize}${search == undefined || search == "" ? "" : `&search=${search}`}${startDate == undefined ? "" : `&startDate=${startDate}`}${endDate == undefined ? "" : `&startDate=${endDate}`}`,
  GET_ID: (component: number) => `/api/${CONTROLLER}/${component}`,
  DELETE: (guid: string) => `/api/${CONTROLLER}/${guid}`,
  DELETE_RANGE: `/api/${CONTROLLER}/delete/range`,
  UPDATE: `/api/${CONTROLLER}`,
  CREATE: `/api/${CONTROLLER}`,
  COMMAND: `/api/${CONTROLLER}/command`,
} as const;
