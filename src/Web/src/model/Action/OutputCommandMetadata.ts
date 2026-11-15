import { OutputCommand } from "../../enum/OutputCommand";

export interface OutputCommandMetadata{
      outputGuid:string;
      command:OutputCommand;
      onTime:number;
      offTime:number;
      repeat:number;
}