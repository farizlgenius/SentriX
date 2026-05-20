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
      public short mode {get; set;}
      public short default_pulse { get; set; } 
      public string type {get; set;} = string.Empty;
 

      public Outputs() { }

      public Outputs(Domain.Entities.Outputs domain) : base(domain.ComponentId,domain.LocationId,domain.IsActive)
      {
            name = domain.Name;
            mac = domain.Mac;
            module_component_id = domain.ModuleComponentId;
            device_component_id = domain.DeviceComponentId;
            output_no = domain.OutputNo;
            model = domain.Model;
            mode = domain.Mode;
            default_pulse = domain.DefaultPulse;
            type = domain.Type;
            this.updated_at = DateTime.UtcNow;
            this.created_at = DateTime.UtcNow;
      }



}

