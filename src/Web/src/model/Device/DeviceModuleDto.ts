import { DeviceModuleModel } from "../../enum/DeviceModuleModel";

export interface DeviceModuleDto {
  guid: string;
  name: string;
  serialNumber: string;
  address: string;
  mac: string;
  port: string;
  firmware: string;
  model: DeviceModuleModel;
  deviceModules: DeviceModuleDto[];
  locationGuid: string;
}
