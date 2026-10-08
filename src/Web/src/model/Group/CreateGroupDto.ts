import { BaseDto } from "../BaseDto";
import { GroupComponentDto } from "./GroupComponentDto";

export interface CreateGroupDto extends BaseDto{
    name:string;
    doors:GroupComponentDto[];
    locationId:number;
    isActive:boolean;
}

