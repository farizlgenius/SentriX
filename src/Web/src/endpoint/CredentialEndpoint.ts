const CONTROLLER = `user`;

export const CredentialEndpoint = {
    POST_SCAN: `/api/${CONTROLLER}/scan`,
    GET_FLAG: `/api/${CONTROLLER}/flag`
} as const;