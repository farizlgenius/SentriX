
import { IntervalDto } from "../Interval/IntervalDto";

export interface TimeZoneDto {
    guid:string ;
    componentId:number;
    name:string;
    intervals:IntervalDto[];
    locationId:number;
    isActive:boolean;
    type:string;
    isDefault:boolean;
}