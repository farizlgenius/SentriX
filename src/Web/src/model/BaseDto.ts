import { Vendor } from "../enum/Vendor";

export interface BaseDto{
  guid:string | null;
  name:string | null;
  vendor:Vendor | null;
}