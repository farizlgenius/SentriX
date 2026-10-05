import { DoorMode } from "../enum/DoorMode";
import { DoorStatus } from "../enum/DoorStatus";
import { InputStatus } from "../enum/InputStatus";
import { ReaderStatus } from "../enum/ReaderStatus";
import { Status } from "../enum/Status";

export interface StatusDto {
  guid: string;
  status: Status | DoorStatus; 
  altr1: InputStatus | ReaderStatus;
  altr2: InputStatus;
  altr3: InputStatus;
  altr4: DoorMode
}
