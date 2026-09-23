import { DeviceComponentConfigurationDto } from "./DeviceComponentConfigurationDto";

export interface ScpConfiguration{
      mac:string;
      locationId:number;
      configurations:DeviceComponentConfigurationDto[];
}