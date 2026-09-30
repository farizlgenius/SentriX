import { InputMode } from "../../enum/InputMode";
import { Vendor } from "../../enum/Vendor";


export interface BgDto {
      slotNo: number;
      mode: InputMode;
      metadata: string;
      vendor: Vendor;
      deviceModuleGuid: string;
}