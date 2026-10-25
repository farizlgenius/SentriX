import { OutputCommand } from "../../enum/OutputCommand";

export interface OutputCommandDto{
      guid:string;
      command:OutputCommand;
      onTime:number;
      offTime:number;
      repeat:number;
}