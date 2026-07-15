
import { AmicoMetadata } from "./AmicoMetadata";


export interface AmicoDtoMetadata  {
  name: string;
  componentId:number;
   mac:string;
  serialNumber: string;
  ip: string;
  port:number;
  fw:string;
  type:string;
  status:string;
  syncedAt: Date;
  locationId:number;
  metadata:AmicoMetadata;
  
}

