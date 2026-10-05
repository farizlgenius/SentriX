import { InputMode } from "../../enum/InputMode";

export interface AeroSensorMetadata{
      mode:InputMode;
      debounce:number;
      holdTime:number;
      dcHeld:number;
}