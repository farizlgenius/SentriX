import { InputMode } from "../../enum/InputMode";
import { Vendor } from "../../enum/Vendor";


export interface SensorDto {
      guid: string;
      slotNo: number;
      mode: InputMode;
      metadata: string;
      vendor: Vendor;
      deviceModuleGuid: string;
}