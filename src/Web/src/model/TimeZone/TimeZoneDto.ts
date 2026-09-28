export interface TimeZoneDto {
  guid: string;
  name: string;
  intervals: string[];
  locationGuid: string;
  isActive: boolean;
  isDefault: boolean;
}
