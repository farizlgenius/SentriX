import { InputMode } from "../../enum/InputMode";
import { Vendor } from "../../enum/Vendor";
import { AeroRexMetadata } from "./AeroRexMetadata";

export interface RexDto {
  slotNo: number;
  mode: InputMode;
  vendor: Vendor;
  deviceModuleGuid:string;
  metadata : string | AeroRexMetadata
}
