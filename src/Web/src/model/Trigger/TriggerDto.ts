import { EntityType } from "../../enum/EntityType";
import { TriggerCommand } from "../../enum/TriggerCommand";

export interface TriggerDto {
    guid:string;
    name:string;
    command:TriggerCommand,
    device:string;
    triggerSource:string;
    triggerType:string;
    triggerCodes:string[];
    triggerArgs:string[];
    firstTriggerVariable:string | null;
    secondTriggerVariable:string | null;
    thirdTriggerVariable:string | null;
    forthTriggerVariable:string | null;
    timeZone:string;
    procedure:string;
    src:string;
    srcType:EntityType;
    isActive:boolean;
    isDefault:boolean;
}