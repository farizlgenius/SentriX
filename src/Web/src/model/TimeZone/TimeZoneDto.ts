export interface TimeZoneDto {
  guid: string;
  name: string;
  intervalGuids: string[];
  locationGuid: string;
  isActive: boolean;
  isDefault: boolean;
}
