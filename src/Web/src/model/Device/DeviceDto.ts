import { Vendor } from "../../enum/Vendor";
import { AeroMetadata } from "./AeroMetadata";
import { AmicoMetadata } from "./AmicoMetadata";
import { DeviceModuleDto } from "./DeviceModuleDto";

export interface DeviceDto {
  guid: string;
  name: string;
  serialNumber: string;
  mac: string;
  ip: string;
  port: string;
  firmware: string;
  vendor: Vendor;
  metadata: AeroMetadata | AmicoMetadata | string;
  syncedAt: Date;
  configurationStatus: string;
  deviceModules: DeviceModuleDto[];
  locationGuid: string;
  isDefault: boolean;
  isActive: boolean;
}
