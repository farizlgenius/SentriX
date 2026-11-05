import { InputMode } from "../../enum/InputMode";
import { Vendor } from "../../enum/Vendor";

export interface InteriorPushButtonDto{
      slotNo: number;
      mode: InputMode;
      vendor: Vendor;
      longPress:number;
      deviceModuleGuid: string;
}