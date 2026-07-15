

export interface DeviceDto  {
  guid:string;
  name: string;
  componentId:number;
  serialNumber: string;
  mac:string;
  ip: string;
  port:string;
  fw:string;
  type:string;
  status:string;
  syncedAt: Date;
  locationId:number;
  metadata:string;
  isDefault:boolean;
  isActive:boolean;
  
}