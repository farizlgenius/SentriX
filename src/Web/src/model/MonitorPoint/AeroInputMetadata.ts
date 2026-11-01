import { InputLatchingMode } from "../../enum/InputLatchingMode";
import { InputLogFunction } from "../../enum/InputLogFunction";
import { InputMode } from "../../enum/InputMode";

export interface AeroInputMetadata{
      mode:InputMode;
      debounce:number;
      holdTime:number;
      lfCode:InputLogFunction;
      latchingMode:InputLatchingMode;
      delayEntry:number;
      delayExit:number;
}

