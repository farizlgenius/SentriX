import { ActionType } from "../../enum/ActionType";

export interface ActionDto {
    guid:string;
    name:string;
    type:ActionType;
    metadata:string;
    isActive:boolean;
    isDefault:boolean;
}