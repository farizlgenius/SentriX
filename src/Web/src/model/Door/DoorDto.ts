import { DoorExtendFeatureType } from "../../enum/DoorExtendFeatureType";
import { DoorType } from "../../enum/DoorType";
import { Vendor } from "../../enum/Vendor";
import { AeroDoorMetadata } from "./AeroDoorMetadata";
import { BgDto } from "./BgDto";
import { BuzzerDto } from "./BuzzerDto";
import { InteriorPushButtonDto } from "./InteriorPushButtonDto";
import { InteriorPushButtonOutputDto } from "./InteriorPushButtonOutputDto";
import { ReaderDto } from "./ReaderDto";
import { RelayDto } from "./RelayDto";
import { RexDto } from "./RexDto";
import { SensorDto } from "./SensorDto";

export interface DoorDto {
  guid: string;
  name: string;
  vendor: Vendor;
  type: DoorType;
  deviceGuid:string;
  deviceName:string;
  metadata: string | AeroDoorMetadata;
  readers: ReaderDto[];
  buzzer: BuzzerDto | null;
  rexes: RexDto[];
  relay:RelayDto | null;
  followerRelay:RelayDto | null;
  sensor: SensorDto | null;
  bg:BgDto | null;
  extendFeatureType:DoorExtendFeatureType;
  interiorPushButton:InteriorPushButtonDto | null;
  interiorPushOutputButton:InteriorPushButtonOutputDto | null;
  locationGuid: string;
  locationName: string;
  isActive: boolean;
  isDefault: boolean;
}
