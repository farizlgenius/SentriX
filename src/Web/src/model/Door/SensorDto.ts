import { InputMode } from "../../enum/InputMode";
import { Vendor } from "../../enum/Vendor";
import { AeroSensorMetadata } from "./AeroSensorMetadata";


export interface SensorDto {
      slotNo: number;
      mode: InputMode;
      metadata: string | AeroSensorMetadata;
      vendor: Vendor;
      deviceModuleGuid: string;
}