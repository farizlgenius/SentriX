import { Vendor } from "../../enum/Vendor";

export interface EventDto{
    guid:string;
    timestamp:Date;
    actor:string;
    module:string;
    eventType:string;
    imageName:string;
    mac:string;
    componentName:string;
    eventCode:string
    remarks:string;
    captureImageName:string;
    vendor:Vendor;
    locationGuid:string;
    isActive:boolean;
    isDefault:boolean;
    status:number;
}