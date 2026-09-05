export interface HolidayDto {
  guid: string;
  name: string;
  start: Date;
  end: Date;
  locationGuid: string;
  isActive: boolean;
  isDefault: boolean;
}
