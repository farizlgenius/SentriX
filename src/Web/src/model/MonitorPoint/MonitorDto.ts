import { BaseDto } from "../BaseDto";
import { AeroInputMetadata } from "./AeroInputMetadata";

export interface MonitorDto
{
    guid:string;
    name:string;
    slotNo:number;
    metadata:string | AeroInputMetadata;
    device:BaseDto;
    deviceModule:BaseDto;
    isActive:boolean;
    isDefault:boolean;
}