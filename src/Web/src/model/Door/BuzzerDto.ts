import { Vendor } from "../../enum/Vendor";
import { AeroBuzzerMetadata } from "./AeroBuzzerMetadata";

export interface BuzzerDto {
  slotNo: number;
  metadata: string | AeroBuzzerMetadata;
  vendor: Vendor;
  deviceModuleGuid:string;
}
