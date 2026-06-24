import { BaseDto } from "../BaseDto";
import { GroupDoorDto } from "./GroupDoorDto";

export interface GroupDto extends BaseDto{
    id:number;
    componentId:number;
    name:string;
    doors:GroupDoorDto[];
    locationId:number;
    isActive:boolean;
}

