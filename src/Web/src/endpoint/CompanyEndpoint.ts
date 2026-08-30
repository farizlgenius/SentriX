const CONTROLLER = `company`;

export const CompanyEndpoint = {
  GET: `/api/${CONTROLLER}`,
  CREATE: `/api/${CONTROLLER}`,
  PAGINATION: (
    pageNumber: number,
    pageSize: number,
    search?: string | undefined,
    startDate?: string | undefined,
    endDate?: string | undefined,
  ) =>
    `/api/${CONTROLLER}/pagination?Page=${pageNumber}&PageSize=${pageSize}${search == undefined || search == "" ? "" : `&Search=${search}`}${startDate == undefined ? "" : `&startDate=${startDate}`}${endDate == undefined ? "" : `&endDate=${endDate}`}`,
  UPDATE: `/api/${CONTROLLER}`,
  DELETE: (guid: string) => `/api/${CONTROLLER}/${guid}`,
  GET_RANGE: `/api/${CONTROLLER}/range`,
  DELETE_RANGE: `/api/${CONTROLLER}/delete/range`,
} as const;
