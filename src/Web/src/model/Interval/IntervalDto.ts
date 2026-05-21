import { DaysInWeekDto } from "./DaysInWeekDto";

export interface IntervalDto {
  id: number;
  componentId:number;
  days: DaysInWeekDto;
  daysDetail:string;
  start: string;
  end: string;
  locationId:number;
  isActive:boolean;
  type:string;
}