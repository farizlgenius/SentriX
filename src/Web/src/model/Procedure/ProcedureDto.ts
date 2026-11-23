import { ActionDto } from "../Action/ActionDto";

export interface ProcedureDto {
    guid:string;
    name:string;
    actions:ActionDto[];
    location:string;
    deviceGuid:string;
    device:string;
    isActive:boolean;
    isDefault:boolean;
}