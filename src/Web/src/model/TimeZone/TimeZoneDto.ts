
import { IntervalDto } from "../Interval/IntervalDto";

export interface TimeZoneDto {
    id:number;
    componentId:number;
    name:string;
    mode:number;
    active:string;
    deactive:string;
    intervals:IntervalDto[];
    locationId:number;
    isActive:boolean;
    type:string;
}