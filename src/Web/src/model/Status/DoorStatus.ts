import { ChangeOfStateEncode } from "../../enum/ChangeOfStateEncode";
import { DoorMode } from "../../enum/DoorMode";
import { ReaderStatus } from "../../enum/ReaderStatus";

export interface DoorStatus{
      guid:string;
      mode:DoorMode;
      readerStatus:ReaderStatus;
      strikeStatus:ChangeOfStateEncode;
      sensorStatus:ChangeOfStateEncode;
      doorStatus:ChangeOfStateEncode;
      rex0Status:ChangeOfStateEncode;
      rex1Status:ChangeOfStateEncode;
      ledModeControl:number;
      accessControlFlag:number;
      alternateReaderStatus:ReaderStatus;
      extendedAccessControlFlag:number;
      extendedFeatureType:number;
      deviceType:number;
      extendedFeatureStatus:number;
}