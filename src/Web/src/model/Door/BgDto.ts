import { InputMode } from "../../enum/InputMode";
import { Vendor } from "../../enum/Vendor";
import { AeroBgMetadata } from "./AeroBgMetadata";


export interface BgDto {
      slotNo: number;
      mode: InputMode;
      metadata: string | AeroBgMetadata;
      vendor: Vendor;
      deviceModuleGuid: string;
}