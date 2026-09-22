import { Vendor } from "../../enum/Vendor";

export interface IdReport {
  guid: string;
  serialNumber: string;
  mac: string;
  vendor:Vendor;
  ip:string;
  port:string;
  firmware:string;
}
