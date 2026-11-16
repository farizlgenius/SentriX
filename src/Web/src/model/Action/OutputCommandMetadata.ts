import { OutputCommand } from "../../enum/OutputCommand";

export interface OutputCommandMetadata{
      command:OutputCommand;
      onTime:number;
      offTime:number;
      repeat:number;
}