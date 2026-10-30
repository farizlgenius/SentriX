import { ChangeOfState } from "../../enum/ChangeOfState";
import { ChangeOfStateEncode } from "../../enum/ChangeOfStateEncode";

export interface OutputStatus{
      guid:string;
      code:ChangeOfState;
      status:ChangeOfStateEncode
      offline:boolean;
      mask:boolean;
      localMask:boolean;
      entryDelayInProgress:boolean;
      notAttached:boolean;
}