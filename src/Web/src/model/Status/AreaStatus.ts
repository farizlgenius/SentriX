import { AreaMode } from "../../enum/AreaMode";

export interface AreaStatus {
      guid:string;
      mode:AreaMode;
      multiOcc:boolean;
      occ:number;
}