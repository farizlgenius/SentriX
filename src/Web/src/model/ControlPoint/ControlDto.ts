import { BaseDto } from "../BaseDto";
import { AeroOutputMetadata } from "./AeroOutputMetadata";

export interface ControlDto 
{
    guid:string;
    name:string;
    slotNo:number;
    metadata:string | AeroOutputMetadata;
    device:BaseDto;
    deviceModule:BaseDto;
    isActive:boolean;
    isDefault:boolean;
}

