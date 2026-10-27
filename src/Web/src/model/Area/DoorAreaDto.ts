import { AntipassBackMode } from "../../enum/AntipassbackMode";
import { DoorType } from "../../enum/DoorType";

export interface DoorAreaDto{
      doorGuid:string;
      type:DoorType;
      antipassbackFrom:AntipassBackMode;
      areaFromGuid:string;
      antipassbackTo:AntipassBackMode;
      areaToGuid:string | null;
}