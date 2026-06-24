import { BaseDto } from "../BaseDto";
import { GroupDoorDto } from "./GroupDoorDto";

export interface CreateGroupDto extends BaseDto{
    name:string;
    doors:GroupDoorDto[];
    locationId:number;
    isActive:boolean;
}

