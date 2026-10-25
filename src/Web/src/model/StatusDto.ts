import { DoorMode } from "../enum/DoorMode";
import { DoorStatus } from "../enum/DoorStatus";
import { InputStatus } from "../enum/InputStatus";
import { ReaderMode } from "../enum/ReaderMode";
import { Status } from "../enum/Status";
import { StrikeMode } from "../enum/StrikeMode";
import { TamperStatus } from "../enum/TamperStatus";

// export interface StatusDto {
//   guid: string;
//   status: Status | DoorStatus; 
//   doorStatus:DoorStatus;
//   altr1: InputStatus | DoorMode;
//   altr2: InputStatus | ReaderMode;
//   altr3: InputStatus | StrikeMode; 
//   altr4: InputStatus;
// }

export interface StatusDto{
  guid:string;
  status:Status;
  doorStatus:DoorStatus;
  inputStatus:InputStatus;
  outputStatus:InputStatus;
  tamper:TamperStatus;
  doorMode:DoorMode;
  readerMode:ReaderMode;
  strkeMode:StrikeMode;
  cabinetTamper:InputStatus;
  powerMonitor:InputStatus;
}