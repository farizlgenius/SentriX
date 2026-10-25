import { Vendor } from "../../enum/Vendor";

export interface OutputDto 
{
    guid:string;
    name:string;
    slotNo:number;
    metadata:string;
    vendor:Vendor;
    isActive:boolean;
    isDefault:boolean;
}

