import { DoorMode } from "../enum/DoorMode";
import { DoorStatus } from "../enum/DoorStatus";
import { InputStatus } from "../enum/InputStatus";
import { ReaderMode } from "../enum/ReaderMode";
import { ReaderStatus } from "../enum/ReaderStatus";
import { Status } from "../enum/Status";
import { StrikeMode } from "../enum/StrikeMode";

export interface StatusDto {
  guid: string;
  status: Status | DoorStatus; 
  altr1: InputStatus | DoorMode;
  altr2: InputStatus | ReaderMode;
  altr3: InputStatus | StrikeMode; 
  altr4: InputStatus;
}
