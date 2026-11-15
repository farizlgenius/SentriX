export interface ProcedureDto {
    guid:string;
    name:string;
    actions:string[];
    location:string;
    deviceGuid:string;
    device:string;
    isActive:boolean;
    isDefault:boolean;
}