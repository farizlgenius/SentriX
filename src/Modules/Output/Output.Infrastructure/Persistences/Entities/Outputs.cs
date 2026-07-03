using System;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;

namespace Output.Infrastructure.Persistences.Entities;

public sealed class Outputs : BaseEntity
{
      public string name { get; set; } = string.Empty;
      public string mac {get; set;} = string.Empty;
      public short module_component_id { get; set; }
      public short device_component_id {get; set;}
      public short output_no {get; set;}
      public string model {get; set;} = string.Empty;
      public short drive_mode {get; set;}
      public short offline_mode {get; set;}
      public short default_pulse { get; set; } 
      public string type {get; set;} = string.Empty;
 

      public Outputs() { }

      public Outputs(Domain.Entities.Outputs domain) : base(domain.ComponentId,domain.LocationId,domain.IsActive,false)
      {
            name = domain.Name;
            mac = domain.Mac;
            module_component_id = domain.ModuleComponentId;
            device_component_id = domain.DeviceComponentId;
            output_no = domain.OutputNo;
            model = domain.Model;
            offline_mode = domain.OfflineMode;
            drive_mode = domain.DriveMode;
            default_pulse = domain.DefaultPulse;
            type = domain.Type;
            this.updated_at = DateTime.UtcNow;
            this.created_at = DateTime.UtcNow;
      }

      public void Update(Domain.Entities.Outputs domain)
      {
            name = domain.Name;
            output_no = domain.OutputNo;
            offline_mode = domain.OfflineMode;
            drive_mode = domain.DriveMode;
            default_pulse = domain.DefaultPulse;
            this.updated_at = DateTime.UtcNow;
      }



}

