import { Vendor } from "../../enum/Vendor";
import { OutputMode } from "./OutputMode";

export interface BuzzerDto {
  guid: string;
  slotNo: number;
  mode: OutputMode;
  metadata: string;
  vendor: Vendor;
}
