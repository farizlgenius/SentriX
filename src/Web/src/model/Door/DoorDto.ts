import { DoorDirection } from "../../enum/DoorDirection";
import { BaseDto } from "../BaseDto";
import { ReaderDto } from "../Reader/ReaderDto";
import { RequestExitDto } from "../RequestExit/RequestExitDto";
import { SensorDto } from "../Sensor/SensorDto";
import { StrikeDto } from "../Strike/StrikeDto";

// export interface DoorDto {
//   id:number;
//   scpId:number;
//   name: string;
//   accessConfig: number;
//   pairDoorNo: number;
//   direction:DoorDirection;
//   acrId:number;

//   // Reader setting for Reader In
//   readers:ReaderDto[] ;
//   readerOutConfiguration:number;

//   // Output setting for strike
//   strk:StrikeDto;

//   // Input setting for sensor
//   sensor:SensorDto ;

//   // Input setting for rex0
//   requestExits:RequestExitDto[];

//   cardFormat:number;
//   antiPassbackMode: number;
//   areaInId: number;
//   areaOutId: number;
//   spareTags: number;
//   accessControlFlags: number;
//   mode:number;
//   modeDesc:string;
//   offlineMode: number;
//   offlineModeDesc:string;
//   defaultMode: number;
//   defaultModeDesc:string;
//   defaultLEDMode: number;
//   preAlarm: number;
//   antiPassbackDelay: number;

//   // Advance Feature
//   strkT2: number;
//   dcHeld2: number;
//   strkFollowPulse: number;
//   strkFollowDelay: number;
//   nExtFeatureType: number;
//   ilPBSio: number;
//   ilPBNumber: number;
//   ilPBLongPress: number;
//   ilPBOutSio: number;
//   ilPBOutNum: number;
//   dfOfFilterTime: number;
//   maskHeldOpen:boolean;
//   maskForceOpen:boolean;
// }

export interface DoorDto{
  id:number;
  componentId:number;
  name:string;
  deviceComponentId:number;
  mac:string;
  doorType:string;
  metadata:AeroDoorMetadata | AmicoDoorMetadata | string;
  locationId:number;
  type:string;
  isActive:boolean;
}

export interface AeroDoorMetadata{
  accessConfig:number;
  readerIn:ReaderIn;
  readerOut:ReaderOut;
  sensor:Sensor;
  relay:Relay;
  rex:Rex;
  altrReader:AltrReader;
  antipassback:Antipassback;
  spare:number;
  accessControlFlag:number;
  offlineMode:number;
  defaultMode:number;
  ledMode:number;
  apbDelay:number;
  relayT2:number;
  heldOpen2:number;
  relayFollowerPulse:number;
  relayFollowerDelay:number;
  extendFeatureType:number;
  interiorPushButtonModuleComponentId:number;
  interiorPushButtonInputNumber:number;
  interiorPushButtonLongPress:number;
  interiorPushButtonOutModuleComponentId:number;
  interiorPushButtonOutRelayNumber:number;
}

export interface AmicoDoorMetadata{

}

export interface ReaderIn{
  readerModuleId:number;
  readerModuleComponentId:number;
  readerNumber:number;
  dataFormat:number;
  keypadMode:number;
  ledDriveMode:number;
  osdpFlag:boolean;
  osdpBaudrate:number;
  osdpDiscover:number;
  osdpTracing:number;
  osdpAddress:number;
  osdpSecureChannel:number;
}

export interface ReaderOut{
  readerModuleId:number;
  readerModuleComponentId:number;
  readerNumber:number;
  dataFormat:number;
  keypadMode:number;
  ledDriveMode:number;
  osdpFlag:boolean;
  osdpBaudrate:number;
  osdpDiscover:number;
  osdpTracing:number;
  osdpAddress:number;
  osdpSecureChannel:number;
}

export interface Sensor{
  sensorModuleId:number;
  sensorModuleComponentId:number;
  sensorNumber:number;
  heldOpenDelay:number;
  sensorMode:number;
  debounce:number;
  holdTime:number;
}

export interface Relay{
  relayModuleId:number;
  relayModuleComponentId:number;
  relayNumber:number;
  relayMin:number;
  relayMax:number;
  relayMode:number;
}

export interface Rex {
  rex0ModuleId:number;
  rex0ModuleComponentId:number;
  rex0Number:number;
  rex1ModuleId:number;
  rex1ModuleComponentId:number;
  rex1Number:number;
  disableRex0Timezone:number;
  disableRex1Timezone:number;
  rex0SensorMode:number;
  rex0Debounce:number;
  rex0HoldTime:number;
  rex1SensorMode:number;
  rex1Debounce:number;
  rex1HoldTime:number;
}

export interface AltrReader{
  altrRdrModuleId:number;
  altrRdrModuleComponentId:number;
  altrRdrNumber:number;
  altrRdrConf:number;
}

export interface Antipassback{
  antipassbackMode:number;
  areaIn:number;
  areaOut:number;
}