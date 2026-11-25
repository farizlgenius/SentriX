import { BaseDto } from "../BaseDto";

export interface LocationDto {
  guid: string;
  name: string;
  description: string;
  country:BaseDto;
  isActive: boolean;
  isDefault: boolean;
}
