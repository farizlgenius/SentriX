import { Vendor } from "../../enum/Vendor";
import { AeroSensorMetadata } from "./AeroSensorMetadata";


export interface SensorDto {
      slotNo: number;
      metadata: string | AeroSensorMetadata;
      vendor: Vendor;
      deviceModuleGuid: string;
}