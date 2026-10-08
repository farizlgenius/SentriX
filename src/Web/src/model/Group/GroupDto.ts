import { GroupComponentDto } from "./GroupComponentDto";

export interface GroupDto {
    guid:string;
    name:string;
    components:GroupComponentDto[];
    isActive:boolean;
    isDefault:boolean;
}

