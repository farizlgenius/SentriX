export interface CreateDeviceStrMetadataDto {
    name:string;
    componentId:number;
    mac:string;
    serialNumber:string;
    ip:string;
    port:number;
    fw:string;
    type:string;
    syncedAt:Date;
    status:string;
    locationId:number;
    metadata:string;
}