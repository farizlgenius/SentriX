const CONTROLLER = `identity/setting`;

export const SettingEndpoint = {
    GET_LED : `/api/${CONTROLLER}/led`,
    GET_BY_ID:(id:number) => `/api/${CONTROLLER}/led/${id}`,
    UPDATE_LED: `/api/${CONTROLLER}/led`
} as const;

