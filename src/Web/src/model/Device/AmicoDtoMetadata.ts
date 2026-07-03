
import { AmicoMetadata } from "./AmicoMetadata";


export interface AmicoDtoMetadata  {

    id:number;
  componentId:number;
  name: string;
  serialNumber: string;
  mac:string;
  ip: string;
  port:string;
  fw:string;
  type:string;
  status:string;
  syncedAt: Date;
  locationId:number;
  metadata:AmicoMetadata;
  
}

