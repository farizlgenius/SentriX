const CONTROLLER = `department`;

export const DepartmentEndpoint = {
  GET: `/api/${CONTROLLER}`,
  GET_BY_COMPANY: (company: string) => `/api/${CONTROLLER}/company/${company}`,
  GET_OPTION_BY_COMPANY: (company: number) =>
    `/api/${CONTROLLER}/option/company/${company}`,
  CREATE: `/api/${CONTROLLER}`,
  PAGINATION_BY_COMPANY: (
    pageNumber: number,
    pageSize: number,
    companyGuid: string,
    search?: string | undefined,
    startDate?: string | undefined,
    endDate?: string | undefined,
  ) =>
    `/api/${CONTROLLER}/pagination/${companyGuid}?Page=${pageNumber}&PageSize=${pageSize}${search == undefined || search == "" ? "" : `&Search=${search}`}${startDate == undefined ? "" : `&StartDate=${startDate}`}${endDate == undefined ? "" : `&EndDate=${endDate}`}`,
  UPDATE: `/api/${CONTROLLER}`,
  DELETE: (guid: string) => `/api/${CONTROLLER}/${guid}`,
  GET_RANGE: `/api/${CONTROLLER}/range`,
  DELETE_RANGE: `/api/${CONTROLLER}/delete/range`,
} as const;
