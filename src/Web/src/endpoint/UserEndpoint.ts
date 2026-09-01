const CONTROLLER = "user";

// CREDENTIAL
export const UserEndpoint = {
  GET: (locationId: number) => `/api/${locationId}/${CONTROLLER}`,
  PAGINATION: (
    pageNumber: number,
    pageSize: number,
    locationGuid?: string | undefined,
    search?: string | undefined,
    startDate?: string | undefined,
    endDate?: string | undefined,
  ) =>
    `/api/${CONTROLLER}/pagination?PageNumber=${pageNumber}&PageSize=${pageSize}${search == undefined || search == "" ? "" : `&search=${search}`}${startDate == undefined ? "" : `&startDate=${startDate}`}${endDate == undefined ? "" : `&startDate=${endDate}`}${locationGuid == "" || locationGuid == undefined ? "" : `&locationGuid=${locationGuid}`}`,
  DELETE: (guid: string) => `/api/${CONTROLLER}/${guid}`,
  UPDATE: `/api/${CONTROLLER}`,
  CREATE: `/api/${CONTROLLER}`,
  SCAN: `/api/${CONTROLLER}/scan`,
  DELETE_RANGE: `/api/${CONTROLLER}/delete/range`,
  UPLOAD: (guid: string) => `/api/${CONTROLLER}/image/upload/${guid}`,
  IMAGE: (guid: string) => `/api/${CONTROLLER}/image/${guid}`,
} as const;
