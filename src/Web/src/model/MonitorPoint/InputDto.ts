import { Vendor } from "../../enum/Vendor";

export interface InputDto
{
    guid:string;
    name:string;
    slotNo:number;
    metadata:string;
    vendor:Vendor;
    deviceGuid:string;
    deviceName:string;
    deviceModuleGuid:string;
    deviceModuleName:string;
    locationGuid:string;
    locationName:string;
    isActive:boolean;
    isDefault:boolean;
}