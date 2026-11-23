import { ActionType } from "../../enum/ActionType";
import { DelayMetadata } from "./DelayMetadata";
import { DoorModeMetadata } from "./DoorModeMetadata";
import { MaskMetadata } from "./InputMaskMetadata";
import { MomentUnlockMetadata } from "./MomentUnlockMetadata";
import { OutputCommandMetadata } from "./OutputCommandMetadata";
import { ProcedureControlCommandMetadata } from "./ProcedureControlCommandMetadata";

export interface ActionDto {
    guid:string;
    name:string;
    deviceGuid:string;
    deviceName:string;
    type:ActionType;
    actionType:string;
    sequence:number;
    actionTarget:ActionTagetDto[] | null;
    targetGuids:string[] | null;
    metadata:string | MaskMetadata | OutputCommandMetadata | DoorModeMetadata | DelayMetadata | MomentUnlockMetadata | ProcedureControlCommandMetadata | null;
    isActive:boolean;
    isDefault:boolean;
}

export interface ActionTagetDto{
    guid:string;
    name:string;
}