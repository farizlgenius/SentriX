import { MonitorType } from "../../enum/MonitorType";



export interface MonitorGroupDto{
    guid:string;
    name:string;
    components:MonitorPointGroupComponent[];
    deviceGuid:string;
    isActive:boolean;
    isDefault:boolean;
}

export interface MonitorPointGroupComponent{
    componentGuid:string;
    type: MonitorType;
}
