import { AreaMode } from "../../enum/AreaMode";
import { AreaMultiOccMode } from "../../enum/AreaMultiOccMode";
import { DoorAreaDto } from "./DoorAreaDto";


export interface AreaDto {
    guid:string;
    name:string;
    multiMode:AreaMultiOccMode;
    mode:AreaMode;
    occMax:number;
    occDown:number;
    occUp:number;
    doors:DoorAreaDto[];
    deviceGuid:string;
    isActive:boolean;
    isDefault:boolean;
}