import { MonitorType } from "../../enum/MonitorType";

export interface MonitorGroupStatus {
      guid:string;
      maskCount:number
      numActive:number;
      activeList:MonitorGroupComponentStatus[]
}

export interface MonitorGroupComponentStatus{
      guid:string;
      type:MonitorType
}

