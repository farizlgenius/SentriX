import { DeviceComponentDto } from "./DeviceComponentDto";

export interface ScpConfiguration{
      mac:string;
      locationId:number;
      configurations:DeviceComponentDto[];
}