const CONTROLLER = `role`;

export const RoleEndpoint = {
  GET: `/api/${CONTROLLER}`,
  GET_BY_LOCATION: (location: number) =>
    `/api/${CONTROLLER}/location/${location}`,
  CREATE: `/api/${CONTROLLER}`,
  PAGINATION: (
    pageNumber: number,
    pageSize: number,
    locationGuid?: string | undefined,
    search?: string | undefined,
    startDate?: string | undefined,
    endDate?: string | undefined,
  ) =>
    `/api/${CONTROLLER}/pagination?Page=${pageNumber}&PageSize=${pageSize}${search == undefined || search == "" ? "" : `&search=${search}`}${startDate == undefined ? "" : `&startDate=${startDate}`}${endDate == undefined ? "" : `&startDate=${endDate}`} ${locationGuid == undefined ? "" : `&locationGuid=${locationGuid}`}`,
  DELETE: (guid: string) => `/api/${CONTROLLER}/${guid}`,
  UPDATE: `/api/${CONTROLLER}`,
  GET_FEATURE: `/api/${CONTROLLER}/feature`,
  DELETE_RANGE: `/api/${CONTROLLER}/list`,
} as const;
