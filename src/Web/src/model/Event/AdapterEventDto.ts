import { EventCommandStatus } from "../../enum/CommandStatus";
import { Vendor } from "../../enum/Vendor";

export interface AdapterEventDto{
    body:string;
    command:string;
    componentId:number;
    guid:string;
    isActive:boolean;
    isDefault:boolean;
    locationGuid:string;
    mac:string;
    name:string;
    reason:string;
    receivedAt?:Date;
    response:string;
    sendAt:Date;
    tag:number;
    vendor:Vendor;
    status:EventCommandStatus;
}