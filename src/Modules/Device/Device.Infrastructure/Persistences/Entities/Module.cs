using System;
using SharedKernel.Domain;

namespace Device.Infrastructure.Persistences.Entities;

public sealed class Module : BaseEntity
{
      public string name {get; set;} = string.Empty;
      public string serial_number {get; set;} = string.Empty;
      public string fw {get; set;} = string.Empty;
      public string mac {get; set;} = string.Empty;
      public short port {get; set;}
      public short address {get; set;}
      public string type {get; set;} = string.Empty;
      public string model {get; set;} = string.Empty;
      public int device_id {get; set;}
      public Devices devices {get; set; } = default!;

      public Module(){}
      public Module(Domain.Entities.Module domain) : base(domain.ComponentId,domain.LocationId,domain.IsActive)
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
