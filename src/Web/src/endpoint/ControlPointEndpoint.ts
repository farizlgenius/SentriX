const CONTROLLER = `output`;

export const ControlPointEndpoint = {
    GET:(location:number) => `/api/${location}/${CONTROLLER}`,
    DELETE :(component:number) => `/api/${CONTROLLER}/${component}`,
    PAGINATION:(pageNumber:number,pageSize:number,locationId?:number | undefined,search?:string | undefined,startDate?:string | undefined,endDate?:string | undefined) => `/api/${CONTROLLER}/pagination?PageNumber=${pageNumber}&PageSize=${pageSize}${search == undefined || search == "" ? "" : `&search=${search}`}${startDate == undefined ? "" : `&startDate=${startDate}`}${endDate == undefined ? "" : `&startDate=${endDate}`}${locationId == undefined ?  "" : `&locationId=${locationId}` }`,
    TRIGGER : (id:number,command:number) => `/api/${CONTROLLER}/${id}?Command=${command}`,
    CREATE : `/api/${CONTROLLER}`,
    UPDATE : `/api/${CONTROLLER}`,
    STATUS :(outputId:number)=> `/api/${CONTROLLER}/status/${outputId}`,
    OUTPUT : (module:number) => `/api/${CONTROLLER}/relay/${module}`,
    GET_RELAY_OP_MODE : (type:string) => `/api/${CONTROLLER}/relay/mode?Type=${type}`,
    DELETE_RANGE: `/api/${CONTROLLER}/delete/range`
} as const;

