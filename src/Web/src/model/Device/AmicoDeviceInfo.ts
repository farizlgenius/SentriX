export interface AmicoDeviceInfo{
      uptime:Uptime;
      time:number;
      memory:Memory;
      license:License;
      network:Network;
      serial:string;
      version:string;
      deviceId:string;
      secBoxVersion:string;
      iDCloudCode:string;
      online:boolean;
      onlineAvaiable:boolean;
}

interface Network{
      mac:string;
      ip:string;
      netmask:string;
      gatewat:string;
      webServerPort:number;
      sslEnabled:boolean;
      dhcpEnabled:boolean;
      tenMbps:boolean;
      dnsPrimary:string;
      dnsSecondary:string;
}

interface License{
      users:number;
      device:number;
      type:number;
}

interface Memory{
      disk:Disk;
      ram:Ram;
}

interface Ram{
      free:number;
      total:number;
}

interface Disk{
      free:number;
      total:number;
}

interface Uptime{
      days:number;
      hours:number;
      minutes:number;
      seconds:number;
}