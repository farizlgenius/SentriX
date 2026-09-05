const CONTROLLER = `time/holiday`;

export const HolidayEndpoint = {
  GET: (location: number) => `/api/${location}/${CONTROLLER}`,
  PAGINATION: (
    pageNumber: number,
    pageSize: number,
    locationGuid?: string | undefined,
    search?: string | undefined,
    startDate?: string | undefined,
    endDate?: string | undefined,
  ) =>
    `/api/${CONTROLLER}/pagination?PageNumber=${pageNumber}&PageSize=${pageSize}${search == undefined || search == "" ? "" : `&search=${search}`}${startDate == undefined ? "" : `&startDate=${startDate}`}${endDate == undefined ? "" : `&startDate=${endDate}`}${locationGuid == "" || locationGuid == undefined ? "" : `&locationGuid=${locationGuid}`}`,
  DELETE_RANGE: `/api/${CONTROLLER}/delete/range`,
  DELETE: (guid: string) => `/api/${CONTROLLER}/${guid}`,
  CREATE: `/api/${CONTROLLER}`,
  UPDATE: `/api/${CONTROLLER}`,
} as const;
