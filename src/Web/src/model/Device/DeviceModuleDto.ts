import { DeviceModuleModel } from "../../enum/DeviceModuleModel";

export interface DeviceModuleDto {
  name: string;
  serialNumber: string;
  mac: string;
  port: string;
  firmware: string;
  model: DeviceModuleModel;
  deviceModules: DeviceModuleDto[];
  locationGuid: string;
}
