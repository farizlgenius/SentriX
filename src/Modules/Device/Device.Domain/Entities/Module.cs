using System;

namespace Device.Domain.Entities;

public sealed class Module
{
      public string Name {get; private set;} = string.Empty;
      public string SerialNumber {get; private set;} = string.Empty;
      public string Fw {get; private set;} = string.Empty;
      public string Mac {get; private set;} = string.Empty;
      public int Port {get; private set;}
      public int Address {get; private set;}
      public string Model {get; private set;} = string.Empty;
      public int DeviceId {get; private set;}

      public Module(){}
      public Module(string name, string serial_number, string fw,int port,int address,string mac,string model,int device_id)
      {
            this.Name = name;
            this.SerialNumber = serial_number;
            this.Fw = fw;
            this.Port = port;
            this.Address = address;
            this.Mac = mac;
            this.Model = model;
            this.DeviceId = device_id;
      }
}


