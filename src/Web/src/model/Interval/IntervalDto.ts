import { DaysInWeekDto } from "./DaysInWeekDto";

export interface IntervalDto {
  guid: string ;
  componentId:number;
  days: DaysInWeekDto;

  start: string;
  end: string;
}