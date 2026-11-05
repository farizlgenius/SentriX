import { InputMode } from "../../enum/InputMode";
import { Vendor } from "../../enum/Vendor";

export interface InteriorPushButtonOutputDto{
      slotNo: number;
      mode: InputMode;
      vendor: Vendor;
      deviceModuleGuid: string;
}