import { CoreEvent } from "../../enum/CoreEvent";
import { CoreEntityDto } from "./CoreEntityDto";

export interface CoreEventDto{
      guid:string;
      entity:CoreEntityDto;
      event:CoreEvent;
}