import { Vendor } from "../../enum/Vendor";
import { AeroBgMetadata } from "./AeroBgMetadata";


export interface BgDto {
      slotNo: number;
      metadata: string | AeroBgMetadata;
      vendor: Vendor;
      deviceModuleGuid: string;
}