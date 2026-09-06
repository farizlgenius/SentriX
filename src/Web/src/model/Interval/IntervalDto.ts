import { DaysInWeekDto } from "./DaysInWeekDto";

export interface IntervalDto {
  guid: string;
  days: DaysInWeekDto;
  start: string;
  end: string;
  locationGuid: string;
  isActive: boolean;
  isDefault: boolean;
}
