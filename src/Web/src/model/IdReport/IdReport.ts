import { Vendor } from "../../enum/Vendor";

export interface IdReport {
  guid: string;
  serialNumber: string;
  mac: string;
  vendor:Vendor;
  ip:string;
  port:number;
  firmware:string;
}
