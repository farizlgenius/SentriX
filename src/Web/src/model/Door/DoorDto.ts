import { DoorType } from "../../enum/DoorType";
import { Vendor } from "../../enum/Vendor";
import { SensorDto } from "../Sensor/SensorDto";
import { BuzzerDto } from "./BuzzerDto";
import { ReaderDto } from "./ReaderDto";
import { RexDto } from "./RexDto";

export interface DoorDto {
  guid: string;
  name: string;
  vendor: Vendor;
  type: DoorType;
  metadata: string;
  readers: ReaderDto[];
  buzzer: BuzzerDto | null;
  rex: RexDto | null;
  sensor: SensorDto | null;
  locationGuid: string;
  locationName: string;
  isActive: boolean;
  isDefault: boolean;
}
