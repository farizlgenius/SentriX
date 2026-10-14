import { ActionType } from "../../enum/ActionType";
import { InputMaskMetadata } from "./InputMaskMetadata";

export interface ActionDto {
    guid:string;
    name:string;
    type:ActionType;
    metadata:string | InputMaskMetadata;
    deviceGuid:string | null;
    isActive:boolean;
    isDefault:boolean;
}