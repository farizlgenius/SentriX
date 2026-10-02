import { Vendor } from "../../enum/Vendor";
import { AeroRexMetadata } from "./AeroRexMetadata";

export interface RexDto {
  slotNo: number;
  vendor: Vendor;
  deviceModuleGuid:string;
  metadata : string | AeroRexMetadata;
  maskGuid:string | null;
}
