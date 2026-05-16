using System;
using SharedKernel.Domain;

namespace Device.Infrastructure.Persistences.Entities;

public sealed class Module : BaseEntity
{
      public string name {get; set;} = string.Empty;
      public string serial_number {get; set;} = string.Empty;
      public string fw {get; set;} = string.Empty;
      public string mac {get; set;} = string.Empty;
      public int port {get; set;}
      public int address {get; set;}
      public string model {get; set;} = string.Empty;
      public int device_id {get; set;}
      public Devices devices {get; set; } = new Devices();

      public Module(){}
      public Module(Domain.Entities.Module domain)
      {
            name = domain.Name;
            serial_number = domain.SerialNumber;
            fw = domain.Fw;
            mac = domain.Mac;
            model = domain.Model;
            device_id = domain.DeviceId;
            this.created_at = DateTime.UtcNow;
            this.updated_at = DateTime.UtcNow;
      }
}
