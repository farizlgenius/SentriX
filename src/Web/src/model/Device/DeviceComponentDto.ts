import { EntityType } from "../../enum/EntityType";

export interface DeviceComponentDto{
    components:Components[]
    isSynced:boolean
}

export interface Components{
    type:EntityType,
    total:number,
    uploaded:number;
    remain:number;
    isSynced:boolean;
}