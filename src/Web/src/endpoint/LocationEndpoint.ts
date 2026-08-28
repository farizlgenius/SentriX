const CONTROLLER = `location`;

export const LocationEndpoint = {
  GET: `/api/${CONTROLLER}`,
  CREATE: `/api/${CONTROLLER}`,
  PAGINATION: (
    page: number,
    pageSize: number,
    search?: string | undefined,
    startDate?: string | undefined,
    endDate?: string | undefined,
    locationGuid?: string,
  ) =>
    `/api/${CONTROLLER}/pagination?Page=${page}&PageSize=${pageSize}${search == undefined || search == "" ? "" : `&Search=${search}`}${startDate == undefined ? "" : `&startDate=${startDate}`}${endDate == undefined ? "" : `&startDate=${endDate}`}${locationGuid !== undefined ? `&LocationGuid=${locationGuid}` : ""}`,
  UPDATE: `/api/${CONTROLLER}`,
  DELETE: (guid: string) => `/api/${CONTROLLER}/${guid}`,
  GET_RANGE: `/api/${CONTROLLER}/list`,
  DELETE_RANGE: `/api/${CONTROLLER}/list`,
  COUNTRY: `/api/${CONTROLLER}/countries`,
} as const;
