import { IntervalDto } from "../Interval/IntervalDto";

export interface TimeZoneDto {
  guid: string;
  name: string;
  intervals: IntervalDto[];
  locationGuid: string;
  isActive: boolean;
  isDefault: boolean;
}
