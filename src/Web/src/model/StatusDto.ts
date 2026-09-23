import { Status } from "../enum/Status";

export interface StatusDto {
  guid: string;
  status: Status; 
  tamper: Status;
  ac: Status;
  batt: Status;
}
