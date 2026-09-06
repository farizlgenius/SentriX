const CONTROLLER = `interval`;

export const IntervalEndpoint = {
  GET: `/api/${CONTROLLER}`,
  PAGINATION: (
    pageNumber: number,
    pageSize: number,
    locationGuid?: string | undefined,
    search?: string | undefined,
    startDate?: string | undefined,
    endDate?: string | undefined,
  ) =>
    `/api/${CONTROLLER}/pagination?PageNumber=${pageNumber}&PageSize=${pageSize}${search == undefined || search == "" ? "" : `&search=${search}`}${startDate == undefined ? "" : `&startDate=${startDate}`}${endDate == undefined ? "" : `&startDate=${endDate}`}${locationGuid == "" || locationGuid == undefined ? `` : `&locationGuid=${locationGuid}`}`,
  LOCATION: (location: number) => `/api/${location}/${CONTROLLER}`,
  CREATE: `/api/${CONTROLLER}`,
  DELETE: (guid: string) => `/api/${CONTROLLER}/${guid}`,
  UPDATE: `/api/${CONTROLLER}`,
  DELETE_RANGE: `/api/${CONTROLLER}/delete/range`,
} as const;
