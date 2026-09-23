import { DeviceConfigurationStatus } from "../../enum/DeviceConfigurationStatus";

export interface MemoryDto{
    id:number;
    type:string;
    driverRecord:number;
    record:number;
    recordSize:number;
    active:number;
    status:DeviceConfigurationStatus;
}