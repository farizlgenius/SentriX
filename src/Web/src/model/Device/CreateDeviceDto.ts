
export interface CreateDeviceDto {
    name:string;
    componentId:number;
    mac:string;
    serialNumber:string;
    fw:string;
    type:string;
    syncedAt:Date;
    status:string;
    locationId:number;
}