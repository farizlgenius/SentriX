import { ActionType } from "../../enum/ActionType";
import { DelayMetadata } from "./DelayMetadata";
import { DoorModeMetadata } from "./DoorModeMetadata";
import { InputMaskMetadata } from "./InputMaskMetadata";
import { OutputCommandMetadata } from "./OutputCommandMetadata";

export interface ActionDto {
    guid:string;
    name:string;
    deviceGuid:string;
    deviceName:string;
    type:ActionType;
    srcGuid:string | null;
    metadata:string | InputMaskMetadata | OutputCommandMetadata | DoorModeMetadata | DelayMetadata | null;
    isActive:boolean;
    isDefault:boolean;
}