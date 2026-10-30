import { ChangeOfState } from "../../enum/ChangeOfState";

export interface InputStatus{
      guid:string;
      status:ChangeOfState;
      offline:boolean;
      mask:boolean;
      localMask:boolean;
      entryDelayInProgress:boolean;
      notAttached:boolean;
}