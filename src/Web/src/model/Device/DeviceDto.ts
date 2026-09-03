import { Vendor } from "../../enum/Vendor";
import { AeroMetadata } from "./AeroMetadata";
import { AmicoMetadata } from "./AmicoMetadata";

export interface DeviceDto {
  guid: string;
  name: string;
  serialNumber: string;
  mac: string;
  ip: string;
  port: string;
  firmware: string;
  vendor: Vendor;
  metadata: AeroMetadata | AmicoMetadata;
  syncedAt: Date;
  configurationStatus: string;
  locationGuid: string;
  isDefault: boolean;
  isActive: boolean;
}
