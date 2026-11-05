import { Vendor } from "../../enum/Vendor";
import { AeroRelayMetadata } from "./AeroRelayMetadata";

export interface RelayDto{
      slotNo:number;
      metadata:string | AeroRelayMetadata;
      vendor:Vendor;
      deviceModuleGuid:string;
}
