import { Vendor } from "../../enum/Vendor";
import { AeroInputMetadata } from "./AeroInputMetadata";

export interface InputDto
{
    guid:string;
    name:string;
    slotNo:number;
    metadata:string | AeroInputMetadata;
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