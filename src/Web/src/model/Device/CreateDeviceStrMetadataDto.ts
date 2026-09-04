import { Vendor } from "../../enum/Vendor";

export interface CreateDeviceStrMetadataDto {
  name: string;
  serialNumber: string;
  mac: string;
  ip: string;
  port: string;
  firmware: string;
  vendor: Vendor;
  metadata: string;
  syncedAt: Date;
  configurationStatus: string;
  locationGuid: string;
}
