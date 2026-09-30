import { Vendor } from "../../enum/Vendor";
import { RelayMode } from "../../enum/RelayMode";

export interface BuzzerDto {
  guid: string;
  slotNo: number;
  mode: RelayMode;
  metadata: string;
  vendor: Vendor;
}
