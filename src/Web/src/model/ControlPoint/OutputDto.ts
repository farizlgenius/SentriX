import { Vendor } from "../../enum/Vendor";
import { AeroOutputMetadata } from "./AeroOutputMetadata";

export interface OutputDto 
{
    guid:string;
    name:string;
    slotNo:number;
    metadata:string | AeroOutputMetadata;
    vendor:Vendor;
    deviceModuleGuid:string;
    isActive:boolean;
    isDefault:boolean;
}

