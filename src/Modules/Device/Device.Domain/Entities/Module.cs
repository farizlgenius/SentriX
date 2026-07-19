using System;
using SharedKernel.Domain;

namespace Device.Domain.Entities;

public sealed class Module : BaseDomainEntity
{
      public string Name {get; private set;} = string.Empty;
      public string SerialNumber {get; private set;} = string.Empty;
      public string Fw {get; private set;} = string.Empty;
      public string Mac {get; private set;} = string.Empty;
      public int Port {get; private set;}
      public int Address {get; private set;}
      public string Type {get; private set;} = string.Empty;
      public string Model {get; private set;} = string.Empty;
      public Guid Device_Guid {get; private set;}

      public Module(Guid guid,short componentId,string name, string serial_number, string fw,int port,int address,string mac,string model,string type,Guid device_guid,int locationId,bool isActive,bool isDefault) : base(guid,locationId,isActive,isDefault)
      {
            this.Name = name;
            this.SerialNumber = serial_number;
            this.Fw = fw;
            this.Port = port;
            this.Address = address;
            this.Mac = mac;
            this.Model = model;
            this.Device_Guid = device_guid;
            this.Type = type;
      }
}


