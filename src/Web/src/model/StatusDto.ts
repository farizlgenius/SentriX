import { InputStatus } from "../enum/InputStatus";
import { Status } from "../enum/Status";

export interface StatusDto {
  guid: string;
  status: Status; 
  tamper: InputStatus;
  ac: InputStatus;
  batt: InputStatus;
}
