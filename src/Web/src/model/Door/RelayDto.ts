import { Vendor } from "../../enum/Vendor";
import { AeroRelayMetadata } from "./AeroRelayMetadata";
import { RelayMode } from "../../enum/RelayMode";

export interface RelayDto{
      guid:string;
      slotNo:number;
      mode:RelayMode;
      metadata:string | AeroRelayMetadata;
      vendor:Vendor;
      deviceModuleGuid:string;
}
