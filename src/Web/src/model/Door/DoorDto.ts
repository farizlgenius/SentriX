export interface AeroDoorDto{
  id:number;
  componentId:number;
  name:string;
  deviceComponentId:number;
  secondComponentId:number;
  mac:string;
  doorType:string;
  metadata:AeroDoorMetadata
  locationId:number;
  type:string;
  isActive:boolean;
}

export interface DoorDto{
  id:number;
  componentId:number;
  name:string;
  deviceComponentId:number;
  secondComponentId:number;
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
  relayDriveMode:number;
  relayOfflineMode:number;
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